using System.ServiceModel;

namespace SecurityTokenServices
{
    [ServiceContract]
    public interface ISecurityTokenService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        string GenerateToken(string username, string saltedUsername, string saltedPassword);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void ExpireToken(string tokenID);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        string UseToken(string tokenID);
    }
}