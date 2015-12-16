using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SecurityTokenServices
{
    public class SecurityTokenManager
    {
        private Dictionary<string, Token> issuedTokens;
        private static readonly RNGCryptoServiceProvider rngCSP = new RNGCryptoServiceProvider();

        public SecurityTokenManager()
        {
            issuedTokens = new Dictionary<string, Token>();
        }

        public string GenerateToken(string username, string saltedUsername, string saltedPassword)
        {
            if(string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username is null or empty!","username");
            }

            if (string.IsNullOrEmpty(saltedUsername))
            {
                throw new ArgumentException("Salted username is null or empty!", "saltedUsername");
            }

            if (string.IsNullOrEmpty(saltedPassword))
            {
                throw new ArgumentException("Salted password is null or empty!", "saltedPassword");
            }

            string salt = GenerateSaltString();

            UserNameSecurityToken usernameToken = new UserNameSecurityToken(saltedUsername, saltedPassword);

            string saltedTokenID = GetHashedString(SHA256.Create(), usernameToken.Id + salt);

            if (issuedTokens.ContainsKey(saltedTokenID))
            {
                throw new Exception("Token already exists!");
            }

            issuedTokens.Add(saltedTokenID, new Token(saltedTokenID, usernameToken, salt, username));

            return saltedTokenID;
        }

        public void ExpireToken(string tokenID)
        {
            if (string.IsNullOrEmpty(tokenID))
            {
                throw new ArgumentException("Token ID can not be null or empty!","tokenID");
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
            if (!issuedTokens.TryGetValue(tokenID, out token))
            {
                throw new Exception("token does not exist!");
            }

            token.Used = true;
            return token.Username;
        }

        public static string GenerateSaltString()
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
