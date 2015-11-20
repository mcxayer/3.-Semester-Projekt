using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class DomainFacade
    {
        private static readonly DomainFacade instance = new DomainFacade();
        public static DomainFacade Instance { get { return instance; } }

        private Dictionary<OperationContext, string> activeClients;

        private DomainFacade()
        {
            activeClients = new Dictionary<OperationContext, string>();
        }

        public bool Connect(string tokenID)
        {
            if(activeClients.ContainsKey(OperationContext.Current))
            {
                return false;
            }

            string username = GeneralService.DomainFacade.Instance.UseToken(tokenID);

            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            activeClients.Add(OperationContext.Current, username);
            return true;
        }

        public bool Disconnect()
        {
            if (!activeClients.ContainsKey(OperationContext.Current))
            {
                return false;
            }

            activeClients.Remove(OperationContext.Current);
            return true;
        }
    }
}
