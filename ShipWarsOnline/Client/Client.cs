using System;
using System.ServiceModel;
using GeneralService;
using System.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        static void Main(string[] args)
        {
            ChannelFactory<IService> channelfactory = new ChannelFactory<IService>("GeneralServiceEndpoint");
            //channelfactory.Credentials.UserName.UserName = "username";
            //channelfactory.Credentials.UserName.Password = "password";
            IService proxy = channelfactory.CreateChannel();

            Console.WriteLine("Dette er Klienten");

            try
            {
                proxy.CreateAccount("hej", "med", "dig");
                Console.WriteLine("Ny bruger skabt!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke skabe ny bruger!");
            }

            String tokenID = null;
            try
            {
                tokenID = proxy.Login("hej", "med");
                Console.WriteLine("Logget ind med token id: " + tokenID);
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
                proxy.Logout(tokenID);
                Console.WriteLine("Logget ud!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke logge ud!");
            }

            Console.Read();
        }
    }

}