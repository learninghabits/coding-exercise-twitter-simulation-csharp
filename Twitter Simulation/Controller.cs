using System;
using System.Collections.Generic;
using System.Linq;

namespace Twitter_Simulation
{
    public class Controller : IController
    {
        public Controller(
            IUser user,
            ITweet tweet,
            IDataPresenter presenter)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (tweet == null) throw new ArgumentNullException("tweet");
            if (presenter == null) throw new ArgumentNullException("presenter");
            _user = user;
            _tweet = tweet;
            _presenter = presenter;
        }

        private IUser _user;
        private ITweet _tweet;
        private IDataPresenter _presenter;

        private List<IUser> Users { get; set; }
        private List<ITweet> Tweets { get; set; }

        private List<IUserTweet> UserTweets { get; set; }
        private Controller ReadUsersFile(string path)
        {
            Users = _user.Read(path);
            if (Users == null)
            {
                throw new Exception("Could not get a list of twitter users for the given path");
            }
            return this;
        }

        private Controller ReadTweetsFile(string path)
        {
            Tweets = _tweet.Read(path);
            if (Tweets == null)
            {
                throw new Exception("Could not get a list of twitter messages for the given path");
            }
            return this;
        }

        private Controller MatchUserTweets()
        {
            UserTweets = new List<IUserTweet>();
            Users.ForEach(user =>
            {
                var combinedTweets = new List<ITweet>();
                combinedTweets = combinedTweets.Concat(Tweets.Where(tweet => tweet.User == user.UserName)
                                                             .ToList()).ToList();
                user.Follows.ForEach(follow =>
                {
                    combinedTweets = combinedTweets.Concat(Tweets.Where(tweet => tweet.User == follow)
                                                             .ToList()).ToList();
                });
                UserTweets.Add(new UserTweet
                {
                    User = user.UserName,
                    CombinedTweets = combinedTweets
                });

            });

            return this;
        }

        private Controller DisplayOutput()
        {
            UserTweets.ForEach(ut =>
            {
                _presenter.Write(ut.User);
                ut.CombinedTweets.ForEach(tweet =>
                {
                    _presenter.Write($"\t  {tweet.User} : {tweet.Message}");
                });
            });
            return this;
        }

        public void Run(string usersFilePath, string tweetsFilePath)
        {
            if (string.IsNullOrWhiteSpace(usersFilePath)) throw new ArgumentNullException("usersFilePath");
            if (string.IsNullOrWhiteSpace(tweetsFilePath)) throw new ArgumentNullException("tweetsFilePath");

            this.ReadUsersFile(usersFilePath)
                .ReadTweetsFile(tweetsFilePath)
                .MatchUserTweets()
                .DisplayOutput();
        }
    }
}
