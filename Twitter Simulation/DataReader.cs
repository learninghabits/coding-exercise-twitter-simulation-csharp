using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Twitter_Simulation
{
    public class DataReader : IDataReader
    {
        public List<string> Read(string path)
        {
            return File.ReadAllLines(path)
                          .ToList();
        }
    }
}
