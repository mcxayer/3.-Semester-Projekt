using System.Collections.Generic;

namespace GameServices
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private Lobby lobby;

        public DomainFacade()
        {
            lobby = new Lobby();
        }

        public void ConnectLobby(string tokenID)
        {
            lobby.Connect(tokenID);
        }

        public void DisconnectLobby()
        {
            lobby.Disconnect();
        }

        public void Matchmake()
        {
            lobby.Matchmake();
        }

        public void CancelMatchmaking()
        {
            lobby.CancelMatchmaking();
        }

        public List<string> GetLobby()
        {
            return lobby.GetLobbyClients();
        }

        public void TakeTurn(int x, int y)
        {
            lobby.TakeTurn(x, y);
        }
    }
}
