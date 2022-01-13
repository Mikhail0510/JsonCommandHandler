using JsonCommandHandler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Commands
{
    internal class CommandExecutor
    {
        private readonly ApplicationDbContext dbContext;
        internal CommandExecutor(ApplicationDbContext dbCnt)
        {
            dbContext = dbCnt;
        }

        internal CommandInfo[] GetRecords()
        {
            return dbContext.Commands.ToArray();
        }

        public List<string> ExtractNewCommands(List<string> files)
        {
            CommandInfo[] executedCommands = GetRecords();
            List<string> result = new List<string>();
            for(int i=0; i<files.Count; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                var oldCommand = executedCommands.Where(x => x.FileName == fileInfo.Name & x.CreationDate == fileInfo.CreationTime).FirstOrDefault();
                if (oldCommand == null)
                {
                    result.Add(files[i]);
                }
                
            }
            return result;
        }

        
    }
}
