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
            ChannelFactory<GeneralService.IService> generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            //channelfactory.Credentials.UserName.UserName = "username";
            //channelfactory.Credentials.UserName.Password = "password";
            GeneralService.IService generalProxy = generalFactory.CreateChannel();

            ChannelFactory<GameService.IService> gameFactory = new ChannelFactory<GameService.IService>("GameServiceEndpoint");
            GameService.IService gameProxy = gameFactory.CreateChannel();

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

            try
            {
                generalProxy.Logout(tokenID);

                // proxyGame.RemoveFromLobby(tokenID); <-- placeholder, since I need the new proxy to GameServices

                Console.WriteLine("Logget ud!");

                if (gameProxy.Disconnect())
                {
                    Console.WriteLine("Afsluttet forbindelse til spilserver!");
                }
            }
            catch
            {
                Console.WriteLine("Kunne ikke logge ud!");
            }

            Console.Read();
        }
    }

}