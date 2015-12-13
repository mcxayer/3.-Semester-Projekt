using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace SecurityTokenService
{
    public class Service : IService
    {
        public string GenerateToken(string username, string saltedUsername, string saltedPassword)
        {
            try
            {
                return DomainFacade.Instance.GenerateToken(username, saltedUsername, saltedPassword);
            }
            catch
            {
                throw new FaultException("Credentials are invalid, or a connection could not be established!");
            }
        }

        public void ExpireToken(string tokenID)
        {
            try
            {
                DomainFacade.Instance.ExpireToken(tokenID);
            }
            catch
            {
                throw new FaultException("Token is not valid!");
            }
        }

        public string UseToken(string tokenID)
        {
            try
            {
                return DomainFacade.Instance.UseToken(tokenID);
            }
            catch
            {
                throw new FaultException("Credentials are invalid, username is already taken, or a connection could not be established!");
            }
        }
    }
}
