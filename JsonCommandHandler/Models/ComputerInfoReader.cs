using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Models
{
    internal class ComputerInfoReader
    {
        internal string GetComputerName()
        {
            return Environment.MachineName.ToString();
        }

        internal string GetNumberOfCores()
        {
            return Environment.ProcessorCount.ToString();
        }
    }
}
