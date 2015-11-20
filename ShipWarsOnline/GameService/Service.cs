using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class Service : IService
    {
        private List<string> lobby = new List<string>();

        public bool Connect(string tokenID)
        {
            return DomainFacade.Instance.Connect(tokenID);
        }

        public bool Disconnect()
        {
            return DomainFacade.Instance.Disconnect();
        }

        public void AddToLobby(string tokenID)
        {
            try
            {
                //lobby.Add(tokenID.username <-- Something that changes tokenID to username);
            }
            catch
            {
                throw new FaultException("Invalid username or token");
            }
        }

        public void RemoveFromLobby(string tokenID)
        {
            try
            {
                //lobby.Remove(tokenID.username <-- Something that changes tokenID to username);
            }
            catch
            {
                throw new FaultException("Invalid username or token");
            }
        }

        public List<string> GetLobby()
        {
            return lobby;
        }
    }
}
