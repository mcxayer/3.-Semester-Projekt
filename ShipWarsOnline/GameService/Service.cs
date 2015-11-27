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
        public bool Connect(string tokenID)
        {
            return DomainFacade.Instance.Connect(tokenID);
        }

        public bool Disconnect()
        {
            return DomainFacade.Instance.Disconnect();
        }

        public List<string> GetLobby()
        {
            return DomainFacade.Instance.GetLobby();
        }

        public void Matchmake()
        {
            DomainFacade.Instance.Matchmake();
        }
    }
}
