using System;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SecurityTokenServices
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private SecurityTokenManager tokenService;

        private DomainFacade()
        {
            tokenService = new SecurityTokenManager();
        }

        public string GenerateToken(string username, string saltedUsername, string saltedPassword)
        {
            return tokenService.GenerateToken(username, saltedUsername, saltedPassword);
        }

        public void ExpireToken(string tokenID)
        {
            tokenService.ExpireToken(tokenID);
        }

        public string UseToken(string tokenID)
        {
            return tokenService.UseToken(tokenID);
        }
    }
}
