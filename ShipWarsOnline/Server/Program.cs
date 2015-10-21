using System;
using System.ServiceModel;
using Service;

namespace Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Server" in both code and config file together.
    public class Server
    {

        static void Main(string[] args)
        {
            ServiceHost server = new ServiceHost(typeof(WcfService));
            server.Open();

            Console.WriteLine("Dette er Serveren");
            Console.ReadLine();
        }

    }
}
