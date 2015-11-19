using System;
using System.ServiceModel;
using GeneralService;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Server
{
    public class Server
    {

        static void Main(string[] args)
        {
            ServiceHost server = new ServiceHost(typeof(GeneralService.Service));
            server.Open();

            Console.WriteLine("Dette er Serveren");

            // http://www.codeproject.com/Articles/37496/TCP-IP-Protocol-Design-Message-Framing
            // http://blogs.msdn.com/b/joncole/archive/2006/03/20/simple-message-framing-sample-for-tcp-socket.aspx

            Console.ReadLine();
        }

    }
}
