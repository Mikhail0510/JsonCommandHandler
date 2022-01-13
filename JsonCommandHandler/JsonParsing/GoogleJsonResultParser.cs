using JsonCommandHandler.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.JsonParsing
{
    internal class GoogleJsonResultParser
    {
        internal List<Book> Parse(string jsonString)
        {
            List<Book> bks = new List<Book>();
            JToken token = JToken.Parse(jsonString);
            JArray books = (JArray)token.SelectToken("items");
            foreach (JToken b in books)
            {
                bks.Add(new Book()
                {
                    title = b["title"].ToString()
                });
            }
            return bks;
        }
    }
}
