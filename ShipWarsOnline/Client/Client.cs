using System;
using System.ServiceModel;
using System.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        static void Main(string[] args)
        {
            var generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            //channelfactory.Credentials.UserName.UserName = "username";
            //channelfactory.Credentials.UserName.Password = "password";
            var generalProxy = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<GameService.IService>(new DuplexedClass(),"GameServiceEndpoint");
            var gameProxy = gameFactory.CreateChannel();

            Console.WriteLine("Dette er Klienten");

            try
            {
                generalProxy.CreateAccount("hej", "med", "dig");
                Console.WriteLine("Ny bruger skabt!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke skabe ny bruger!");
            }

            String tokenID = null;
            try
            {
                tokenID = generalProxy.Login("hej", "med");
                Console.WriteLine("Logget ind med token id: " + tokenID);

                if (gameProxy.Connect(tokenID))
                {
                    Console.WriteLine("Forbundet til spilserver!");
                }

                //proxyGame.AddToLobby(tokenID); <-- placeholder, since I need the new proxy to GameServices
            }
            catch (FaultException)
            {
                Console.WriteLine("Kunne ikke logge ind!");
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("\nTryk Enter for at afslutte session...");
            Console.ReadLine();

            try
            {
                if (gameProxy.Disconnect())
                {
                    Console.WriteLine("Afsluttet forbindelse til spilserver!");
                }
                else
                {
                    Console.WriteLine("Kunne ikke afslutte forbindelse til spilserver!");
                }

                // proxyGame.RemoveFromLobby(tokenID); <-- placeholder, since I need the new proxy to GameServices

                generalProxy.Logout(tokenID);
                Console.WriteLine("Logget ud!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke logge ud!");
            }

            Console.Read();
        }

        private class DuplexedClass : GameService.ICallback
        {
            public void OnPlayerConnected(string username)
            {
                Console.WriteLine(string.Format("Player {0} connected to the game server!",username));
            }
        }
    }

}