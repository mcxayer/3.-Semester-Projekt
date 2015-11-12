using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DomainFacade
    {
        public static readonly DomainFacade Instance = new DomainFacade();

        private DatabaseFacade databaseFacade;

        private DomainFacade()
        {
            databaseFacade = new DatabaseFacade();
        }

        public string Login(string username, string password, string email)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password can not be null or empty!");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            SHA256 sha256Encryption = SHA256.Create();
            string saltString = SecurityTokenService.Instance.GenerateSaltString();

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, username + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            if (!Verify(usernameHash, passwordHash, email))
            {
                return null;
            }

            return SecurityTokenService.Instance.GenerateToken(usernameHash, passwordHash);
        }

        public void CreateAccount(string username, string password, string email)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password can not be null or empty!");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            SHA256 sha256Encryption = SHA256.Create();
            string saltString = SecurityTokenService.Instance.GenerateSaltString();

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, username + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            databaseFacade.CreateAccount(usernameHash, passwordHash, email, saltString);
        }

        public bool Verify(string username, string password, string email)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password can not be null or empty!");
            }

            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email can not be null or empty!");
            }

            SHA256 sha256Encryption = SHA256.Create();
            string saltString = databaseFacade.GetSalt(email);

            string usernameHash = SecurityTokenService.GetHashedString(sha256Encryption, username + saltString);
            string passwordHash = SecurityTokenService.GetHashedString(sha256Encryption, password + saltString);

            return databaseFacade.Verify(usernameHash, passwordHash);
        }
    }
}
