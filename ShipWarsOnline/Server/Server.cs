using System;
using System.ServiceModel;
using Service;
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
            ServiceHost server = new ServiceHost(typeof(WcfService));
            server.Open();

            Console.WriteLine("Dette er Serveren");

            SHA256 sha256Encryption = SHA256.Create();
            String saltString = SecurityTokenService.Instance.GenerateSaltString();

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, "TestUser" + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, "MySecretPassword" + saltString);
            DomainFacade.Instance.DatabaseAccess.AddUser(usernameHash, passwordHash, saltString);

            // http://www.codeproject.com/Articles/37496/TCP-IP-Protocol-Design-Message-Framing
            // http://blogs.msdn.com/b/joncole/archive/2006/03/20/simple-message-framing-sample-for-tcp-socket.aspx

            Console.ReadLine();
        }

    }
}
