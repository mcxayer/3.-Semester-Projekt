using System.ServiceModel;

namespace GeneralServices
{
    [ServiceContract]
    public interface IGeneralService
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