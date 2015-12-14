using System;
using System.ServiceModel;

namespace Server
{
    public class Application
    {
        static void Main(string[] args)
        {
            ServiceHost tokenHost = new ServiceHost(typeof(SecurityTokenServices.SecurityTokenService));
            tokenHost.Open();
            Console.WriteLine("Token Services open!");

            ServiceHost generalHost = new ServiceHost(typeof(GeneralServices.GeneralService));
            generalHost.Open();
            Console.WriteLine("General Services open!");

            ServiceHost gameHost = new ServiceHost(typeof(GameServices.GameService));
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
