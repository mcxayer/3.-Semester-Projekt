﻿using System.ServiceModel;

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