using System.ServiceModel;

namespace GeneralServices
{
    public class GeneralService : IGeneralService
    {
        public string Login(string username, string password)
        {
            try
            {
                return DomainFacade.Instance.Login(username, password);
            }
            catch
            {
                throw new FaultException("Credentials are invalid or a connection could not be established!");
            }
        }

        public void Logout(string tokenId)
        {
            try
            {
                DomainFacade.Instance.Logout(tokenId);
            }
            catch
            {
                throw new FaultException("Token is not valid!");
            }
        }

        public void CreateAccount(string username, string password, string email)
        {
            try
            {
                DomainFacade.Instance.CreateAccount(username, password, email);
            }
            catch
            {
                throw new FaultException("Credentials are either invalid, username is already taken or a connection could not be established!");
            }
        }
    }
}
