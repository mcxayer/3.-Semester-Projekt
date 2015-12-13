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
        void ConnectLobby(string tokenID);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void DisconnectLobby();

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
        void OnTurnTaken(GameCellImpactDTO cellImpact);

        [OperationContract(IsOneWay = true)]
        void OnShipDestroyed(GameShipDestroyedDTO ship);

        [OperationContract(IsOneWay = true)]
        void OnPlayerWon(int playerIndex);
    }

    [DataContract]
    public class GameInitStateDTO
    {
        [DataMember]
        public int GridSize { get; set; }

        [DataMember]
        public ShipData[] Ships { get; set; }

        [DataMember]
        public int PlayerIndex { get; set; }

        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public string OpponentName { get; set; }
    }

    [DataContract]
    public class GameCellImpactDTO
    {
        [DataMember]
        public int AffectedPosX { get; set; }

        [DataMember]
        public int AffectedPosY { get; set; }

        [DataMember]
        public int PlayerIndex { get; set; }

        [DataMember]
        public CellType Type { get; set; }
    }

    [DataContract]
    public class GameShipDestroyedDTO
    {
        [DataMember]
        public int StartPosX { get; set; }

        [DataMember]
        public int StartPosY { get; set; }

        [DataMember]
        public int EndPosX { get; set; }

        [DataMember]
        public int EndPosY { get; set; }

        [DataMember]
        public int PlayerIndex { get; set; }
    }
}
