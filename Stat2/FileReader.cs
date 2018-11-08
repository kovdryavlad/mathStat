using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stat2
{
    public class FileReader
    {
        public static List<string[]> ReadFile(string path)
        {
            int columns = 0;

            List<string[]> rows = new List<string[]>();

            using (StreamReader r = new StreamReader(path))
            {
                string str;
                while ((str = r.ReadLine()) != null)
                {
                    var splitted = str.Split(new[] { '\t', ' ', ';'}, StringSplitOptions.RemoveEmptyEntries);
                    rows.Add(splitted);
                }
            }

            return rows;
        }
    }
}
