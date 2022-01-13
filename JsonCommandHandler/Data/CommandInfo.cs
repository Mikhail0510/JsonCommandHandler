using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Data
{
    public class CommandInfo
    {
        internal string FileName { get; set; }
        internal DateTime CreationDate { get; set; }
    }
}
