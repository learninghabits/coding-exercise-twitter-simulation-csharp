using System;

namespace Twitter_Simulation
{
    public class ConsoleDataPresenter : IDataPresenter
    {
        public void Write(string data)
        {
            Console.WriteLine(data);
        }
    }
}
