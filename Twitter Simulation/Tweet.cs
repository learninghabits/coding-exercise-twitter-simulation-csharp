using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Twitter_Simulation
{
    public class Tweet : ITweet
    {
        public Tweet(IDataReader dataReader)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            _dataReader = dataReader;
        }

        private List<string> _lines;
        private IDataReader _dataReader;

        private List<ITweet> MapToTweetObject()
        {
            var tweets = new List<ITweet>();
            _lines.ForEach(line =>
            {
                var items = line.Split('>');
                var message = items[1].Trim();
                tweets.Add(new Tweet(_dataReader)
                {
                    User = items[0],
                    Message = message
                });
            });

            return tweets;
        }
        private Tweet ExtractValidLines()
        {
            var maximumCharatersAllowedPerMessage = 140;
            var pattern = @"@\w+>\s\w+(\s\w +)*"; //regex to macth pattern @USER1> tweet -- a better pattern is needed (my regex skills do suck)
            var regex = new Regex(pattern);
            _lines = _lines.Where(line => regex.IsMatch(line) && line.Trim().Length <= maximumCharatersAllowedPerMessage)
                           .ToList();
            return this;
        }
        private Tweet ReadFile(string path)
        {
            _lines = _dataReader.Read(path);
            if (_lines == null)
            {
                throw new Exception("Data Reader could not return data for the given path");
            }
            return this;
        }
        public string Message { get; private set; }
        public string User { get; private set; }
        public List<ITweet> Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");          
            return ReadFile(path)
                       .ExtractValidLines()
                       .MapToTweetObject();
        }
    }
}