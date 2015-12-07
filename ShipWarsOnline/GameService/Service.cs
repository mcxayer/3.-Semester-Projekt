using System.Collections.Generic;
using System.ServiceModel;

namespace GameService
{
    //[ServiceBehavior(InstanceContextMode= InstanceContextMode.PerSession)]
    public class Service : IService
    {
        public bool Connect(string tokenID)
        {
            try
            {
                return DomainFacade.Instance.ConnectLobby(tokenID);
            }
            catch
            {
                throw new FaultException("Could not connect!");
            }
        }

        public bool Disconnect()
        {
            try
            {
                return DomainFacade.Instance.DisconnectLobby();
            }
            catch
            {
                throw new FaultException("Could not disconnect!");
            }
        }

        public List<string> GetLobby()
        {
            try
            {
                return DomainFacade.Instance.GetLobby();
            }
            catch
            {
                throw new FaultException("Could not get lobby!");
            }
        }

        public void Matchmake()
        {
            try
            {
                DomainFacade.Instance.Matchmake();
            }
            catch
            {
                throw new FaultException("Could not start matchmaking!");
            }
        }

        public void CancelMatchmaking()
        {
            try
            {
                DomainFacade.Instance.CancelMatchmaking();
            }
            catch
            {
                throw new FaultException("Could not cancel matchmaking!");
            }
        }

        public void TakeTurn(int x, int y)
        {
            try
            {
                DomainFacade.Instance.TakeTurn(x, y);
            }
            catch
            {
                throw new FaultException("Could not take turn!");
            }
        }

        public GameStateDTO GetGameState()
        {
            //try
            //{
            //    return DomainFacade.Instance.GetGameState();
            //}
            //catch
            //{
            //    throw new FaultException("Could not get game state!");
            //}

            return DomainFacade.Instance.GetGameState();
        }
    }
}
