using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

        [OperationContract(IsOneWay = true)]
        //[FaultContract(typeof(FaultException))]
        void Matchmake();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void CancelMatchmaking();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        GameStateDTO GetGameState();

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void TakeTurn(int x, int y);
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void OnPlayerConnected(string player);

        [OperationContract(IsOneWay = true)]
        void OnPlayerDisconnected(string player);

        [OperationContract(IsOneWay = true)]
        void OnPlayerMatchmade();

        [OperationContract(IsOneWay = true)]
        void OnPlayerEnteredMatchmaking();

        [OperationContract(IsOneWay = true)]
        void OnPlayerExitedMatchmaking();

        [OperationContract(IsOneWay = true)]
        void OnLobbyUpdated();

        [OperationContract(IsOneWay = true)]
        void OnGameUpdated(GameDeltaStateDTO deltaState);
    }

    [DataContract]
    public class GameStateDTO
    {
        [DataMember]
        public SeaGridData playerGrid;

        [DataMember]
        public SeaSquareData[][] opponentCells;
    }

    [DataContract]
    public class GameDeltaStateDTO
    {
        [DataMember]
        public int affectedX;

        [DataMember]
        public int affectedY;
    }
}
