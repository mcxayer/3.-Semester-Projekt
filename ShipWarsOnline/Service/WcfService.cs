using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

// http://www.codeproject.com/KB/WCF/WCFWPFChat.aspx?msg=3513805
// http://gamedev.stackexchange.com/questions/46575/how-should-multiplayer-games-handle-authentication
namespace Service
{
    public class WcfService : IWcfService
    {
        private static Dictionary<string, UserNameSecurityToken> tokens;
        private static RNGCryptoServiceProvider rngCSP;

        static WcfService()
        {
            tokens = new Dictionary<string, UserNameSecurityToken>();
            rngCSP = new RNGCryptoServiceProvider();
        }

        // En bruger identificeres ud fra salted usernameHash, salted passwordHash og salted tokenId
        // http://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
        public string Login(string username, string password, string email)
        {
            return DomainFacade.Instance.Login(username, password, email);
        }

        public bool Logout(string tokenId)
        {
            SecurityTokenService.Instance.ExpireToken(tokenId);
            return true;
        }

        public bool CreateAccount(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username is null or empty!");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password is null or empty!");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email is null or empty!");
            }


            DomainFacade.Instance.CreateAccount(username, password, email);

            return true;
        }
    }
}
