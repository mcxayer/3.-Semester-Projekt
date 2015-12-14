using GameServices;
using GeneralServices;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace Battleship
{
    public class ServiceFacade
    {
        private IGeneralService generalService;
        private IGameService gameService;

        private string tokenId;

        public ServiceFacade(GameServices.IGameServiceCallback callback)
        {
            var generalFactory = new ChannelFactory<IGeneralService>("GeneralServiceEndpoint");
            generalService = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<IGameService>(callback, "GameServiceEndpoint");
            gameService = gameFactory.CreateChannel();
        }

        #region general services
        public void Login(string username, string password)
        {
            tokenId = generalService.Login(username, password);
        }

        public void Logout()
        {
            generalService.Logout(tokenId);
        }

        public void CreateAccount(string username, string password, string email)
        {
            generalService.CreateAccount(username, password, email);
        }

        #endregion

        #region game services

        public List<string> GetLobby()
        {
            return gameService.GetLobby();
        }

        public void ConnectLobby()
        {
            gameService.ConnectLobby(tokenId);
        }

        public void DisconnectLobby()
        {
            gameService.DisconnectLobby();
        }

        public void Matchmake()
        {
            gameService.Matchmake();
        }

        public void CancelMatchmaking()
        {
            gameService.CancelMatchmaking();
        }

        public void TakeTurn(int x, int y)
        {
            gameService.TakeTurn(x, y);
        }

        #endregion
    }
}
