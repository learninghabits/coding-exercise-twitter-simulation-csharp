using System.Collections.Generic;

namespace Twitter_Simulation
{
    public interface IDataReader
    {
        List<string> Read(string path);
    }
}
