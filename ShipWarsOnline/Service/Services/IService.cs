using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IdentityModel.Tokens;

namespace GeneralService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        string Login(string username, string password);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Logout(string tokenId);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void CreateAccount(string username, string password, string email);
    }
}