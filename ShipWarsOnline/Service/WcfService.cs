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
        private static Dictionary<String, UserNameSecurityToken> tokens;
        private static RNGCryptoServiceProvider rngCSP;

        static WcfService()
        {
            tokens = new Dictionary<String, UserNameSecurityToken>();
            rngCSP = new RNGCryptoServiceProvider();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public int DoMath(String tokenId, int a, int b)
        {
            return VerifyToken(tokenId) ? a + b : -1;
        }

        // En bruger identificeres ud fra salted usernameHash, salted passwordHash og salted tokenId
        // http://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
        public String Login(string username, string password)
        {
            SHA256 sha256Encryption = SHA256.Create();
            String saltString = SecurityTokenService.Instance.GenerateSaltString();

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, username + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            if (!DomainFacade.Instance.DatabaseAccess.Verify(usernameHash, passwordHash))
            {
                return null;
            }

            return SecurityTokenService.Instance.GenerateToken(usernameHash, passwordHash);
        }

        public bool Logout(string tokenId)
        {
            SecurityTokenService.Instance.ExpireToken(tokenId);
            return true;
        }

        private bool VerifyToken(String tokenId)
        {
            if(String.IsNullOrEmpty(tokenId))
            {
                return false;
            }

            if(!tokens.ContainsKey(tokenId))
            {
                return false;
            }

            // Check time here

            // Check against username

            return true;
        }
    }
}
