using JsonCommandHandler.Commands;
using JsonCommandHandler.Commands.CreateFolderCommand;
using JsonCommandHandler.Commands.GoogleCommand;
using JsonCommandHandler.CreateFolderCommand;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.ServiceCollectionExtensions
{
    internal delegate ICommandHandler<GoogleSearchCommandHandler> ServiceResolverG(string key);
    internal delegate ICommandHandler<CreateFolderCommandHandler> ServiceResolverF(string key);

    public static class CommandExtensions
    {
        public static Type commandType { get; set; }
        public static Type commandHandlerType { get; set; }

        public static string jsonCommand { get; set; }

        public static IServiceCollection Define(this IServiceCollection services, Action<DefineExtensionMethodOptions> defineOptions)
        {
            
            services.Configure(defineOptions);

            if(commandType == typeof(GoogleSearchCommand))
            {
                services.AddScoped<ICommandHandler<GoogleSearchCommand>, GoogleSearchCommandHandler>();
            }else if(commandType == typeof(FolderCommand))
            {
                services.AddScoped<ICommandHandler<FolderCommand>, CreateFolderCommandHandler>();
            }

            //services.AddTransient<ServiceResolverG>(serviceProvider => key =>
            //{
            //    switch (key)
            //    {
            //        case "google-search-command":
            //            return serviceProvider.GetService<GoogleSearchCommandHandler>();
            //    }
            //});

            return services;
        }

    }


    public class DefineExtensionMethodOptions { 

        public DefineExtensionMethodOptions OfType<CommandType>()
        {
            CommandExtensions.commandType = typeof(CommandType);
            return this;
        }

        public DefineExtensionMethodOptions HandledBy<CommandHandlerType>()
        {
            CommandExtensions.commandHandlerType = typeof(CommandHandlerType);
            return this;
        }

        public DefineExtensionMethodOptions MappedToMessageType(string command)
        {
            CommandExtensions.jsonCommand = command;
            return this;
        }
    }
}
