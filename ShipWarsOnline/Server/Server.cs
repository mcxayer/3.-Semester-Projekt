using System;
using System.ServiceModel;

namespace Server
{
    public class Server
    {
        static void Main(string[] args)
        {
            ServiceHost generalHost = new ServiceHost(typeof(GeneralService.Service));
            generalHost.Open();
            Console.WriteLine("General Services open!");

            ServiceHost gameHost = new ServiceHost(typeof(GameService.Service));
            gameHost.Open();
            Console.WriteLine("Game Services open!");
            Console.WriteLine();

            Console.ReadLine();
        }

    }
}
