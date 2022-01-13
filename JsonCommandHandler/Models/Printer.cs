using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Models
{
    internal class Printer
    {
        private readonly ILogger logger;

        public Printer(ILogger logger)
        {
            this.logger = logger;
        }

        internal void Print(List<Book> books)
        {
            for (int i = 0; i < books.Count; i++)
            {
                logger.LogInformation(books[i].title);
            }

        }
    }
}
