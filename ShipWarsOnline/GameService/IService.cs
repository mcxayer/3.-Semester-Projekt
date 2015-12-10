using ShipWarsOnline.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace GameService
{
    [ServiceContract(CallbackContract = typeof(ICallback),SessionMode = SessionMode.Required)]
    public interface IService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Connect(string tokenID);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Disconnect();

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
        void TakeTurn(int x, int y);
    }

    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void OnPlayerConnected();

        [OperationContract(IsOneWay = true)]
        void OnPlayerDisconnected();

        [OperationContract(IsOneWay = true)]
        void OnPlayerFailedConnecting();

        [OperationContract(IsOneWay = true)]
        void OnPlayerMatchmade();

        [OperationContract(IsOneWay = true)]
        void OnPlayerEnteredMatchmaking();

        [OperationContract(IsOneWay = true)]
        void OnPlayerCancelledMatchmaking();

        [OperationContract(IsOneWay = true)]
        void OnLobbyUpdated();

        [OperationContract(IsOneWay = true)]
        void OnGameInit(GameInitStateDTO initState);

        [OperationContract(IsOneWay = true)]
        void OnCellImpact(GameCellImpactDTO cellImpact);

        [OperationContract(IsOneWay = true)]
        void OnShipRevealed(GameShipDTO ship);
    }

    [DataContract]
    public class GameInitStateDTO
    {
        [DataMember]
        public int GridSize { get; set; }

        [DataMember]
        public ShipType[] Ships { get; set; }

        [DataMember]
        public int PlayerIndex { get; set; }
    }

    [DataContract]
    public class GameCellImpactDTO
    {
        [DataMember]
        public int AffectedX { get; set; }

        [DataMember]
        public int AffectedY { get; set; }

        [DataMember]
        public int PlayerIndex { get; set; }
    }

    [DataContract]
    public class GameShipDTO
    {
        [DataMember]
        public int AffectedX { get; set; }

        [DataMember]
        public int AffectedY { get; set; }
    }
}
