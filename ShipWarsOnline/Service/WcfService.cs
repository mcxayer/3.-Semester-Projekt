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
        private static Dictionary<String, SecurityToken> tokens;

        static WcfService()
        {
            tokens = new Dictionary<String, SecurityToken>();

            //SHA256 sha256Encryption = SHA256.Create();
            //DatabaseFacade.Instance.AddUser(GetHashedString(sha256Encryption, "TestUser"), GetHashedString(sha256Encryption, "MySecretPassword"));
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

            string usernameHash = GetHashedString(sha256Encryption, username);
            string passwordHash = GetHashedString(sha256Encryption, password);

            //if (!DatabaseFacade.Instance.Verify(usernameHash, passwordHash))
            //{
            //    return null;
            //}

            SecurityToken token = new UserNameSecurityToken(usernameHash, passwordHash);
            tokens.Add(token.Id, token);

            return token.Id;
        }

        private String GetHashedString(HashAlgorithm hashAlgorithm, String s)
        {
            byte[] sHash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            StringBuilder hashSB = new StringBuilder();
            for (int i = 0; i < sHash.Length; i++)
            {
                hashSB.Append(sHash[i]);
            }

            return hashSB.ToString();
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

            return true;
        }
    }
}
