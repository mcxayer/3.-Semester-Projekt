using System;
using System.ServiceModel;
using Service;
using System.Security.Cryptography;

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

            SHA256 sha256Encryption = SHA256.Create();
            DatabaseFacade.Instance.AddUser(DatabaseFacade.GetHashedString(sha256Encryption, "TestUser"), DatabaseFacade.GetHashedString(sha256Encryption, "MySecretPassword"));

            Console.ReadLine();
        }

    }
}
