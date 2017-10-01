using System;

namespace Twitter_Simulation
{
    class App
    {
        static void Main(string[] args)
        {
            try
            {
                var dataReader = new DataReader();
                var dataPresenter = new ConsoleDataPresenter();     
                var controller = new Controller(new User(dataReader), new Tweet(dataReader), dataPresenter);
                var usersPath = (args.Length > 0 && !string.IsNullOrEmpty(args[0])) ? args[0] : "user.txt";
                var tweetsPath = (args.Length > 1 && !string.IsNullOrEmpty(args[1])) ? args[1] : "tweet.txt";
                controller.Run(usersPath, tweetsPath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
