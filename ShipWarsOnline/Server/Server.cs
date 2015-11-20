using System;
using System.ServiceModel;
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
            ServiceHost generalHost = new ServiceHost(typeof(GeneralService.Service));
            generalHost.Open();
            Console.WriteLine("General Services open!");

            ServiceHost gameHost = new ServiceHost(typeof(GameService.Service));
            gameHost.Open();
            Console.WriteLine("Game Services open!");

            // http://www.codeproject.com/Articles/37496/TCP-IP-Protocol-Design-Message-Framing
            // http://blogs.msdn.com/b/joncole/archive/2006/03/20/simple-message-framing-sample-for-tcp-socket.aspx

            Console.ReadLine();
        }

    }
}
