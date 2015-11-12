using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SecurityTokenService
    {
        private static readonly SecurityTokenService instance = new SecurityTokenService();
        public static SecurityTokenService Instance { get { return instance; } }

        private Dictionary<String, UserNameSecurityToken> issuedTokens;
        private Dictionary<String, String> tokenSalts;
        private RNGCryptoServiceProvider rngCSP;

        public SecurityTokenService()
        {
            issuedTokens = new Dictionary<string, UserNameSecurityToken>();
            tokenSalts = new Dictionary<string, string>();
            rngCSP = new RNGCryptoServiceProvider();
        }

        // https://msdn.microsoft.com/en-us/library/system.servicemodel.security.securitycredentialsmanager(v=vs.110).aspx  
        // https://msdn.microsoft.com/en-us/library/ms730868(v=vs.110).aspx
        // https://msdn.microsoft.com/en-us/library/vstudio/aa702565(v=vs.100).aspx
        // https://msdn.microsoft.com/en-us/library/system.security.cryptography.rngcryptoserviceprovider.aspx
        // http://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
        public String GenerateToken(String saltedUsername, String saltedPassword)
        {
            // Genererer salt for denne session
            String saltString = GenerateSaltString();

            UserNameSecurityToken token = new UserNameSecurityToken(saltedUsername, saltedPassword);

            String saltedToken = GetHashedString(SHA256.Create(), token.Id + saltString);

            issuedTokens.Add(saltedToken, token);
            tokenSalts.Add(saltedToken, saltString);

            return saltedToken;
        }

        public void ExpireToken(string saltedTokenID)
        {
            if (String.IsNullOrEmpty(saltedTokenID))
            {
                throw new ArgumentException("saltedTokenID can not be null or empty!");
            }

            issuedTokens.Remove(saltedTokenID);
            tokenSalts.Remove(saltedTokenID);
        }

        public bool VerifyToken(String saltedTokenID, String tokenID)
        {
            if(String.IsNullOrEmpty(saltedTokenID))
            {
                throw new ArgumentException("saltedTokenID can not be null or empty!");
            }

            if (String.IsNullOrEmpty(tokenID))
            {
                throw new ArgumentException("tokenID can not be null or empty!");
            }

            UserNameSecurityToken issuedToken;
            if(issuedTokens.TryGetValue(saltedTokenID,out issuedToken))
            {
                if(!issuedToken.Id.Equals(tokenID))
                {
                    return false;
                }
            }

            String salt;
            if(tokenSalts.TryGetValue(saltedTokenID,out salt))
            {

                if(!GetHashedString(SHA256.Create(),tokenID + salt).Equals(saltedTokenID))
                {
                    return false;
                }
            }

            return true;
        }

        public String GenerateSaltString()
        {
            byte[] salt = new byte[32];
            rngCSP.GetBytes(salt);
            return Encoding.UTF8.GetString(salt);
        }

        public static String GetHashedString(HashAlgorithm hashAlgorithm, String s)
        {
            byte[] hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(s));

            StringBuilder hashSB = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                hashSB.Append(hash[i]);
            }

            return hashSB.ToString();
        }

        private bool VerifyToken(String tokenId)
        {
            if (String.IsNullOrEmpty(tokenId))
            {
                return false;
            }

            if (!issuedTokens.ContainsKey(tokenId))
            {
                return false;
            }

            // Check time here

            // Check against username

            return true;
        }
    }
}
