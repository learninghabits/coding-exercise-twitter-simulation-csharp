using System.Collections.Generic;

namespace Twitter_Simulation
{
    public interface ITweet
    {
        string Message { get; }
        string User { get; }
        List<ITweet> Read(string path);
    }
}