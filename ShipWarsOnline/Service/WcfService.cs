using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

// http://www.codeproject.com/KB/WCF/WCFWPFChat.aspx?msg=3513805
// http://gamedev.stackexchange.com/questions/46575/how-should-multiplayer-games-handle-authentication
namespace Service
{
    public class WcfService : IWcfService
    {
        // http://www.codeproject.com/Articles/704865/Salted-Password-Hashing-Doing-it-Right
        // http://stackoverflow.com/questions/647172/what-are-the-pros-and-cons-of-using-an-email-address-as-a-user-id
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
