using System;
using System.ServiceModel;
using Service;
using System.IdentityModel.Tokens;

namespace Client
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Client" in both code and config file together.
    public class Client
    {
        static void Main(string[] args)
        {
            ChannelFactory<IWcfService> channelfactory = new ChannelFactory<IWcfService>("WcfServiceEndpoint");
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

            int math = proxy.DoMath(tokenID, 4, 8);
            Console.WriteLine("4 + 8 = " + math);
            Console.ReadLine();
        }
    }

}