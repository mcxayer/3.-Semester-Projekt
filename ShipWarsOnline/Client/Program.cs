using System;
using System.ServiceModel;
using Service;

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
            Console.WriteLine(proxy.DoMath(4, 8));
            Console.ReadLine();
        }
    }

}