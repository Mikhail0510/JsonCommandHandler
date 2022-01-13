using JsonCommandHandler.AllowedParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.CreateFolderCommand
{
    public class FolderCommand
    {
        public LocationOptions LocationOptions { get; set; }
        public string DirectoryName { get; set; }
        public string FileName { get; set; }
        public Content Content { get; set; }
    }
}
