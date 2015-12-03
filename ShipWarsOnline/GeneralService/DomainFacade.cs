using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GeneralService
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private DatabaseFacade databaseFacade;
        private SecurityTokenService tokenService;

        private DomainFacade()
        {
            databaseFacade = new DatabaseFacade();
            tokenService = new SecurityTokenService();
        }

        public string Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password can not be null or empty!");
            }

            SHA256 sha256Encryption = SHA256.Create();
            string saltString = databaseFacade.GetSalt(username);

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, username + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            if (!Verify(username, passwordHash))
            {
                throw new ArgumentException("Could not login!");
            }

            // Check if already logged in

            return tokenService.GenerateToken(username,usernameHash, passwordHash);
        }

        public void Logout(string tokenId)
        {
            tokenService.ExpireToken(tokenId);
        }

        public void CreateAccount(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password can not be null or empty!");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            SHA256 sha256Encryption = SHA256.Create();
            string saltString = tokenService.GenerateSaltString();
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            if(Verify(username,passwordHash))
            {
                throw new Exception("Username already taken!");
            }

            databaseFacade.CreateAccount(username, Encoding.UTF8.GetBytes(passwordHash), email, Encoding.UTF8.GetBytes(saltString));
        }

        bool Verify(string username, string passwordHash)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentException("password hash can not be null or empty!");
            }

            return databaseFacade.Verify(username, Encoding.UTF8.GetBytes(passwordHash));
        }

        public string UseToken(string tokenID)
        {
            return tokenService.UseToken(tokenID);
        }
    }
}
