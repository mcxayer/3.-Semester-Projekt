using SecurityTokenServices;
using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace GeneralServices
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private DatabaseFacade databaseFacade;
        private ISecurityTokenService tokenService;

        private DomainFacade()
        {
            databaseFacade = new DatabaseFacade();

            try
            {
                var tokenFactory = new ChannelFactory<ISecurityTokenService>("TokenServiceEndpoint");
                tokenService = tokenFactory.CreateChannel();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
            string saltString = databaseFacade.GetUserSalt(username);

            string saltedUsernameHash = SecurityTokenManager.GetHashedString(sha256Encryption, username + saltString);
            string saltedPasswordHash = SecurityTokenManager.GetHashedString(sha256Encryption, password + saltString);

            if (!VerifyUser(username, saltedPasswordHash))
            {
                throw new ArgumentException("Could not login!");
            }

            return tokenService.GenerateToken(username,saltedUsernameHash, saltedPasswordHash);
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
            string saltString = SecurityTokenManager.GenerateSaltString();
            string passwordHash = SecurityTokenManager.GetHashedString(sha256Encryption, password + saltString);

            if(VerifyUser(username,passwordHash))
            {
                throw new Exception("Username already taken!");
            }

            databaseFacade.CreateAccount(username, Encoding.UTF8.GetBytes(passwordHash), email, Encoding.UTF8.GetBytes(saltString));
        }

        bool VerifyUser(string username, string passwordHash)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username can not be null or empty!");
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentException("password hash can not be null or empty!");
            }

            return databaseFacade.VerifyUser(username, Encoding.UTF8.GetBytes(passwordHash));
        }

        public string UseToken(string tokenID)
        {
            return tokenService.UseToken(tokenID);
        }
    }
}
