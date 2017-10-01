using System.Collections.Generic;

namespace Twitter_Simulation
{
    public class UserTweet : IUserTweet
    {
        public string User { get; set; }
        public List<ITweet> CombinedTweets { get; set; }
    }
}