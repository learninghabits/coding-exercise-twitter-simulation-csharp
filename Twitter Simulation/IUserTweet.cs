using System.Collections.Generic;

namespace Twitter_Simulation
{
    public interface IUserTweet
    {
        List<ITweet> CombinedTweets { get; set; }
        string User { get; set; }
    }
}