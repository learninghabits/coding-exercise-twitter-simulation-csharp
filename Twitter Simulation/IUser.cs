using System.Collections.Generic;

namespace Twitter_Simulation
{
    public interface IUser
    {
        List<string> Follows { get; }
        string UserName { get; }

        List<IUser> Read(string path);
        void MergeFollows(List<string> items);
    }
}