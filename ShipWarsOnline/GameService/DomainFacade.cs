using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
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

        public bool ConnectLobby(string tokenID)
        {
            return lobby.Connect(tokenID);
        }

        public bool DisconnectLobby()
        {
            return lobby.Disconnect();
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

        public GameStateDTO GetGameState()
        {
            return lobby.GetGameState();
        }
    }
}
