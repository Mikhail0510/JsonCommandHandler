using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Models
{
    internal static class DirectoryNameGenerator
    {
        public static string ReturnNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
