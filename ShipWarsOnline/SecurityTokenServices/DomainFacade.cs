namespace SecurityTokenServices
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private SecurityTokenManager tokenManager;

        private DomainFacade()
        {
            tokenManager = new SecurityTokenManager();
        }

        public string GenerateToken(string username, string saltedUsername, string saltedPassword)
        {
            return tokenManager.GenerateToken(username, saltedUsername, saltedPassword);
        }

        public void ExpireToken(string tokenID)
        {
            tokenManager.ExpireToken(tokenID);
        }

        public string UseToken(string tokenID)
        {
            return tokenManager.UseToken(tokenID);
        }
    }
}
