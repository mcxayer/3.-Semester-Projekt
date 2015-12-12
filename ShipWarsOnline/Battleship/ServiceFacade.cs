using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using GameService;

namespace Battleship
{
    public class ServiceFacade
    {
        private GeneralService.IService generalService;
        private GameService.IService gameService;

        private string tokenId;

        public ServiceFacade(GameService.ICallback callback)
        {
            var generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            generalService = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<GameService.IService>(callback, "GameServiceEndpoint");
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                gameService = gameFactory.CreateChannel();
            }));
        }

        #region general services
        public bool Login(string username, string password)
        {
            tokenId = generalService.Login(username, password);

            return !string.IsNullOrEmpty(tokenId);
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

        public void Connect()
        {
            gameService.Connect(tokenId);
        }

        public void Disconnect()
        {
            gameService.Disconnect();
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
