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

        public string Login(string username, string password)
        {
            return generalService.Login(username, password);
        }

        public void Logout(string tokenId)
        {
            generalService.Logout(tokenId);
        }

        public void CreateAccount(string username, string password, string email)
        {
            generalService.CreateAccount(username, password, email);
        }

        public List<string> GetLobby()
        {
            return gameService.GetLobby();
        }

        public bool Connect(string tokenId)
        {
            return gameService.Connect(tokenId);
        }

        public bool Disconnect()
        {
            return gameService.Disconnect();
        }

        public void Matchmake()
        {
            gameService.Matchmake();
        }

        public void CancelMatchmaking()
        {
            gameService.CancelMatchmaking();
        }

        public GameStateDTO GetGameState()
        {
            return gameService.GetGameState();
        }
    }
}
