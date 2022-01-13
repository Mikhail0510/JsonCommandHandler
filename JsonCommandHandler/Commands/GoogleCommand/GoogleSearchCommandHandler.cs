using Google.Apis.Services;
using System;
using System.Threading.Tasks;
using Google.Apis.Discovery;
using Google.Apis.Discovery.v1;
using Google.Apis.Discovery.v1.Data;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml;
using JsonCommandHandler.Models;
using JsonCommandHandler.JsonParsing;
using JsonCommandHandler.Data;

namespace JsonCommandHandler.Commands.GoogleCommand
{
    internal class GoogleSearchCommandHandler : ICommandHandler<GoogleSearchCommand>
    {
        private readonly ILogger _logger;

        internal GoogleSearchCommandHandler(ILogger<Program> Logger)
        {
            _logger = Logger;
        }

        public async Task HandleAsync(GoogleSearchCommand command)
        {
            string query = GetGoogleQueryFromString(command.SearchCriterion);
            string uri = PrepareGoogleSearchUri(query);

            var service = new DiscoveryService(new BaseClientService.Initializer
            {
                ApplicationName = "Discovery Sample",
                ApiKey = GoogleSearchCredentials.apikey,
            });

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(uri),
            };

            string jsonResult = String.Empty;

            using (var response = await service.HttpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                jsonResult = await response.Content.ReadAsStringAsync();
            }

            GoogleJsonResultParser parser = new GoogleJsonResultParser();
            List<Book> books = parser.Parse(jsonResult);
            books = HandleSearchParams(books, command.DisplayLinks);
            Printer printer = new Printer(_logger);
            printer.Print(books);
        }

        private List<Book> HandleSearchParams(List<Book> books, DisplayLinks linkNumber)
        {
            if (linkNumber == DisplayLinks.FIRST)
            {
                return new List<Book>() {
                    books.First()
                };
            }
            if(linkNumber == DisplayLinks.FIRST_FIVE)
            {
                return books.GetRange(0, 5);
            }
            if(linkNumber == DisplayLinks.FIRST_TEN)
            {
                return books.GetRange(0, 10);
            }
            return books;
        }

        private string GetGoogleQueryFromString(string str)
        {
            string[] words = str.Split(' ');
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < words.Length; i++)
            {
                stringBuilder.Append(words[i]);
                stringBuilder.Append('+');
            }

            return stringBuilder.ToString().TrimEnd('+');
        }

        private string PrepareGoogleSearchUri(string googleQuery)
        {
            return String.Format("https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}", GoogleSearchCredentials.apikey, GoogleSearchCredentials.SearchEngineIdentif, googleQuery);
        }

        
    }

}
