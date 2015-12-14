using System;
using System.Collections.Generic;
using System.ServiceModel;
using GeneralServices;

namespace GameServices
{
    public class GameService : IGameService
    {
        public void ConnectLobby(string tokenID)
        {
            try
            {
                DomainFacade.Instance.ConnectLobby(tokenID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not connect!");
            }
        }

        public void DisconnectLobby()
        {
            try
            {
                DomainFacade.Instance.DisconnectLobby();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not disconnect!");
            }
        }

        public List<string> GetLobby()
        {
            try
            {
                return DomainFacade.Instance.GetLobby();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not get lobby!");
            }
        }

        public void Matchmake()
        {
            try
            {
                DomainFacade.Instance.Matchmake();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not start matchmaking!");
            }
        }

        public void CancelMatchmaking()
        {
            try
            {
                DomainFacade.Instance.CancelMatchmaking();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not cancel matchmaking!");
            }
        }

        public void TakeTurn(int x, int y)
        {
            try
            {
                DomainFacade.Instance.TakeTurn(x, y);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new FaultException("Could not take turn!");
            }
        }
    }
}
