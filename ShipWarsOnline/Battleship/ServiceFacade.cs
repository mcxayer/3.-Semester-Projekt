using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Battleship
{
    public class ServiceFacade
    {
        private static readonly ServiceFacade instance = new ServiceFacade();
        public static ServiceFacade Instance { get { return instance; } }

        public event Action<string> HandlePlayerConnected;
        public event Action<string> HandlePlayerDisconnected;

        private GeneralService.IService generalService;
        private GameService.IService gameService;

        private ServiceFacade()
        {
            var generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            generalService = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<GameService.IService>(new CallbackHandler(this), "GameServiceEndpoint");
            gameService = gameFactory.CreateChannel();
        }

        public string Login(string username, string password)
        {
            return generalService.Login(username,password);
        }

        // Måske skal logout laves om til ikke at tage imod en token.
        // Eller så skal login og logout smeltes sammen til RequestToken.
        public void Logout(string tokenId)
        {
            generalService.Logout(tokenId);
        }

        public void CreateAccount(string username, string password, string email)
        {
            generalService.CreateAccount(username, password, email);
        }

        private class CallbackHandler : GameService.ICallback
        {
            ServiceFacade facade;

            public CallbackHandler(ServiceFacade facade)
            {
                this.facade = facade;
            }

            public void OnPlayerConnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} connected to the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.HandlePlayerConnected(player);
                }
            }

            public void OnPlayerDisconnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} disconnected from the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.HandlePlayerDisconnected(player);
                }
            }
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
    }
}
