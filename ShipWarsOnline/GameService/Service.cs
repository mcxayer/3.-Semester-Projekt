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

        public void DoStuff()
        {
            throw new NotImplementedException();
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
