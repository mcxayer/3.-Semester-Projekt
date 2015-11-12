using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IdentityModel.Tokens;

namespace Service
{
    [ServiceContract]
    public interface IWcfService
    {
        [OperationContract]
        string Login(string username, string password, string email);

        [OperationContract]
        bool Logout(string tokenId);

        [OperationContract]
        bool CreateAccount(string username, string password, string email);
    }
}