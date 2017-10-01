using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Twitter_Simulation
{
    public class User : IUser
    {
        public User(IDataReader dataReader)
        {
            if (dataReader == null) throw new ArgumentNullException("dataReader");
            _dataReader = dataReader;
        }

        private List<string> _lines;
        private IDataReader _dataReader;
        private List<IUser> _users;

        private List<IUser> SortByUserName()
        {
            return _users.OrderBy(user => user.UserName)
                        .ToList();
        }
        
        private User MapToUserObjects()
        {
            _users = new List<IUser>();
            _lines.ForEach(line =>
            {
                line = line.Replace(" follows", ",");
                var items = line.Split(',')
                                .Select(i => i.Trim())
                                .ToList();
                var user = items[0];
                items.RemoveAt(0);
                var duplicated = _users.SingleOrDefault(u => u.UserName == user);
                if (duplicated != null)
                {
                    var indx = _users.IndexOf(duplicated);                    
                    _users[indx].MergeFollows(items);
                }
                else
                {
                    _users.Add(new User(_dataReader)
                    {
                        UserName = user,
                        Follows = items
                    });
                }
            });

            return this;
        }

        private User ExtractValidLines()
        {
            var pattern = @"@\w+\s(follows)\s(@\w +)*(\,\s@\w +)*";
            var regex = new Regex(pattern); //regex to macth pattern @USER1 follows @USER2, @USER3
            _lines = _lines.Where(line => regex.IsMatch(line)).ToList();
            return this;
        }

        private User ReadFile(string path)
        {
            _lines = _dataReader.Read(path);
            if (_lines == null)
            {
                throw new Exception("Data Reader could not return data for the given path");
            }
            return this;
        }
        public string UserName { get; private set; }

        List<string> _follows;
        public List<string> Follows
        {
            get
            {
                return _follows.Distinct().ToList();
            }
            private set
            {
                _follows = value;
            }
        }
        
        public List<IUser> Read(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return ReadFile(path)
                       .ExtractValidLines()
                       .MapToUserObjects()
                       .SortByUserName();
        }
        
        public void MergeFollows(List<string> items)
        {
            Follows = Follows.Concat(items).ToList();
        }
    }
}