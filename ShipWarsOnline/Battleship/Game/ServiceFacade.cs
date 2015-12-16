using GameServices;
using GeneralServices;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Battleship
{
    public class ServiceFacade
    {
        private IGeneralService generalService;
        private IGameService gameService;

        private string tokenID;

        public bool CreateConnection(IGameServiceCallback callback)
        {
            return CreateConnection(callback,"localhost");
        }

        public bool CreateConnection(IGameServiceCallback callback, string ipAddress)
        {
            if(callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            if(ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            if(ipAddress.Equals(string.Empty))
            {
                return false;
            }

            var generalFactory = new ChannelFactory<IGeneralService>("GeneralServiceEndpoint");
            generalFactory.Endpoint.Address = new EndpointAddress(string.Format("http://{0}:8001/GeneralService", ipAddress));
            generalService = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<IGameService>(callback, "GameServiceEndpoint");
            generalFactory.Endpoint.Address = new EndpointAddress(string.Format("net.tcp://{0}:8002/GameService", ipAddress));
            gameService = gameFactory.CreateChannel();

            return true;
        }

        #region general services
        public void Login(string username, string password)
        {
            tokenID = generalService.Login(username, password);
        }

        public void Logout()
        {
            generalService.Logout(tokenID);
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
            gameService.ConnectLobby(tokenID);
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
