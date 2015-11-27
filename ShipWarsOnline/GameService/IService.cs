using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    [ServiceContract(CallbackContract = typeof(ICallback),SessionMode = SessionMode.Required)]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool Connect(string tokenID);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool Disconnect();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<string> GetLobby();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Matchmake();
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void OnPlayerConnected(string player);

        [OperationContract(IsOneWay = true)]
        void OnPlayerDisconnected(string player);

        [OperationContract(IsOneWay = true)]
        void OnPlayerMatchmade(string gameId);

        [OperationContract(IsOneWay = true)]
        void OnPlayerEnteredMatchmaking();

        [OperationContract(IsOneWay = true)]
        void OnPlayerExitedMatchmaking();
    }
}
