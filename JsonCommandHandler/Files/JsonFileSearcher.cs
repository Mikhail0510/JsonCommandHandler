using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonCommandHandler.Files
{
    internal class JsonFileSearcher
    {
        public List<string> GetFiles(string[] files)
        {
            List<string> result = new List<string>();
            for(int i = 0; i < files.Length; i++)
            {
                FileInfo info = new FileInfo(files[i]);
                if(info.Extension == ".json")
                {
                    result.Add(files[i]);
                }
            }
            return result;
        }
    }
}
