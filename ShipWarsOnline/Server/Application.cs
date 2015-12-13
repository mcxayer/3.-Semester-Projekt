using System;
using System.ServiceModel;

namespace Server
{
    public class Application
    {
        static void Main(string[] args)
        {
            ServiceHost tokenHost = new ServiceHost(typeof(SecurityTokenService.Service));
            tokenHost.Open();
            Console.WriteLine("Token Services open!");

            ServiceHost generalHost = new ServiceHost(typeof(GeneralService.Service));
            generalHost.Open();
            Console.WriteLine("General Services open!");

            ServiceHost gameHost = new ServiceHost(typeof(GameService.Service));
            gameHost.Open();
            Console.WriteLine("Game Services open!");

            Console.WriteLine();

            Console.ReadLine();
            tokenHost.Close();
            generalHost.Close();
            gameHost.Close();
        }

    }
}
