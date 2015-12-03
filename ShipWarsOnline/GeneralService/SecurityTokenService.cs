using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GeneralService
{
    public class SecurityTokenService
    {
        private Dictionary<string, Token> issuedTokens;
        private RNGCryptoServiceProvider rngCSP;

        public SecurityTokenService()
        {
            issuedTokens = new Dictionary<string, Token>();
            rngCSP = new RNGCryptoServiceProvider();
        }

        // https://msdn.microsoft.com/en-us/library/system.servicemodel.security.securitycredentialsmanager(v=vs.110).aspx  
        // https://msdn.microsoft.com/en-us/library/ms730868(v=vs.110).aspx
        // https://msdn.microsoft.com/en-us/library/vstudio/aa702565(v=vs.100).aspx
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider.aspx
        // http://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
        public string GenerateToken(string username, string saltedUsername, string saltedPassword)
        {
            string salt = GenerateSaltString();

            UserNameSecurityToken usernameToken = new UserNameSecurityToken(saltedUsername, saltedPassword);

            string saltedTokenID = GetHashedString(SHA256.Create(), usernameToken.Id + salt);

            if(issuedTokens.ContainsKey(saltedTokenID))
            {
                throw new Exception("Token already exists!");
            }

            issuedTokens.Add(saltedTokenID, new Token(saltedTokenID,usernameToken,salt,username));

            return saltedTokenID;
        }

        public void ExpireToken(string tokenID)
        {
            if (string.IsNullOrEmpty(tokenID))
            {
                throw new ArgumentException("tokenID can not be null or empty!");
            }

            issuedTokens.Remove(tokenID);
        }

        public string UseToken(string tokenID)
        {
            if (string.IsNullOrEmpty(tokenID))
            {
                throw new ArgumentException("tokenID can not be null or empty!");
            }

            Token token;
            if(!issuedTokens.TryGetValue(tokenID,out token))
            {
                throw new Exception("token does not exist!");
            }

            token.Used = true;
            return token.Username;
        }

        public string GenerateSaltString()
        {
            byte[] salt = new byte[32];
            rngCSP.GetBytes(salt);
            return Encoding.UTF8.GetString(salt);
        }

        public static string GetHashedString(HashAlgorithm hashAlgorithm, string s)
        {
            byte[] hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            StringBuilder hashSB = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                hashSB.Append(hash[i]);
            }

            return hashSB.ToString();
        }

        private class Token
        {
            public string TokenID { get; private set; }
            public string Salt { get; private set; }
            public string Username { get; private set; }
            public UserNameSecurityToken UsernameToken { get; private set; }

            public bool Used { get; set; }

            public Token(string tokenID, UserNameSecurityToken usernameToken, string salt, string username)
            {
                TokenID = tokenID;
                Salt = salt;
                Username = username;
                UsernameToken = usernameToken;
            }
        }
    }
}
