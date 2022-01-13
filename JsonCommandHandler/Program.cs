using JsonCommandHandler.Files;
using JsonCommandHandler.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using JsonCommandHandler.JsonParsing;
using JsonCommandHandler.Commands.GoogleCommand;
using JsonCommandHandler.Models;
using JsonCommandHandler.CreateFolderCommand;
using JsonCommandHandler.Commands.CreateFolderCommand;
using JsonCommandHandler.Data;
using Microsoft.EntityFrameworkCore;
using JsonCommandHandler.ServiceCollectionExtensions;

public class Program
{
    private static ApplicationDbContext _appDbContext;

    public static void Main(string[] args)
    {
        var builder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(configure => configure.AddConsole())
                    .AddTransient<Program>();
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Server=localhost;Database=JsonCommandStat;Trusted_Connection=True;"));
                services.Define(command => 
                    command.OfType<GoogleSearchCommand>().HandledBy<GoogleSearchCommandHandler>().MappedToMessageType("google-search-command")
                );
                services.Define(command =>
                    command.OfType<FolderCommand>().HandledBy<CreateFolderCommandHandler>().MappedToMessageType("create-folder-with-file-command")
                );

            }).UseConsoleLifetime();

        var host = builder.Build();

        string WatchDestination = Path.Combine(Environment.CurrentDirectory, "JsonCommands");

        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                if (Directory.Exists(WatchDestination))
                {
                    Console.WriteLine("Watching destination: " + WatchDestination);
                    JsonFileSearcher jsonSearcher = new JsonFileSearcher();
                    var logger = services.GetService<ILogger<Program>>();
                    _appDbContext = services.GetService<ApplicationDbContext>();
                    CommandExecutor commandExecutor = new CommandExecutor(_appDbContext);
                    DynamicJsonParser cmdParser = new DynamicJsonParser(logger);

                    while (true)
                    {
                        string[] files = Directory.GetFiles(WatchDestination);
                        List<string> jsonFiles = jsonSearcher.GetFiles(files);
                        List<string> NotExecutedCommands = commandExecutor.ExtractNewCommands(jsonFiles);
                        if (NotExecutedCommands.Count > 0)
                        {
                            for(int i = 0; i < NotExecutedCommands.Count; i++)
                            {
                                string jsonContent = File.ReadAllText(NotExecutedCommands[i]);
                                object obj = cmdParser.Parse(jsonContent);
                                Type objectType = obj.GetType();
                                if(objectType == typeof(GoogleSearchCommand))
                                {
                                    GoogleSearchCommand command = (GoogleSearchCommand)obj;

                                    //var google = services.GetService<GoogleSearchCommandHandler>();
                                    //google.HandleAsync(command);

                                    GoogleSearchCommandHandler handler = new GoogleSearchCommandHandler(logger);
                                    handler.HandleAsync(command);
                                }
                                if (objectType == typeof(FolderCommand))
                                {
                                    CreateFolderCommandHandler handler = new CreateFolderCommandHandler();
                                    FolderCommand command = (FolderCommand)obj;
                                    handler.HandleAsync(command);
                                }
                            }
                            Console.WriteLine("Handled " + NotExecutedCommands.Count + " commands.");
                            SaveNewCommandsAsCompleted(NotExecutedCommands);
                        }
                        else
                        {
                            Console.WriteLine("No commands " + DateTime.Now.ToString("HH:mm:ss dd.MM.yy"));
                        }
                        Thread.Sleep(5000);
                    }
                }
                else
                {
                    Console.WriteLine("Destination " + WatchDestination + " not found");
                    throw new DirectoryNotFoundException(WatchDestination);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: "+ex.Message);
            }
        }

    }

    public static void SaveNewCommandsAsCompleted(List<string> files)
    {
        for (int i = 0; i < files.Count; i++)
        {
            FileInfo fileInfo = new FileInfo(files[i]);
            _appDbContext.Commands.Add(new CommandInfo()
            {
                CreationDate = fileInfo.CreationTime,
                FileName = fileInfo.Name
            });
        }
        _appDbContext.SaveChanges();
    }

    

}

