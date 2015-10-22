using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Service
{
    public class WcfService : IWcfService
    {
        private static Dictionary<String, UserNameSecurityToken> tokens;

        static WcfService()
        {
            tokens = new Dictionary<String, UserNameSecurityToken>();
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public int DoMath(String tokenId, int a, int b)
        {
            return VerifyToken(tokenId) ? a + b : -1;
        }

        public String Login(string username, string password)
        {
            SHA256 sha256Encryption = SHA256.Create();

            string usernameHash = DatabaseFacade.GetHashedString(sha256Encryption, username);
            string passwordHash = DatabaseFacade.GetHashedString(sha256Encryption, password);

            if (!DatabaseFacade.Instance.Verify(usernameHash, passwordHash))
            {
                return null;
            }

            UserNameSecurityToken token = new UserNameSecurityToken(usernameHash, passwordHash);
            tokens.Add(token.Id, token);

            return token.Id;
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
