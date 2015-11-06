using System;
using System.ServiceModel;
using Service;
using System.IdentityModel.Tokens;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Client" in both code and config file together.
    public class Client
    {
        static void Main(string[] args)
        {
            ChannelFactory<IWcfService> channelfactory = new ChannelFactory<IWcfService>("WcfServiceEndpoint");
            //channelfactory.Credentials.UserName.UserName = "username";
            //channelfactory.Credentials.UserName.Password = "password";
            IWcfService proxy = channelfactory.CreateChannel();

            Console.WriteLine("Dette er Clienten");

            String tokenID = null;
            try
            {
                tokenID = proxy.Login("username", "password");
                if (String.IsNullOrEmpty(tokenID))
                {
                    Console.WriteLine("Kunne ikke logge ind!");
                    return;
                }

                Console.WriteLine("Logget ind med token id: " + tokenID);
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if(proxy.Logout(tokenID))
            {
                Console.WriteLine("Logget ud!");
            }

            Console.ReadLine();
        }
    }

}