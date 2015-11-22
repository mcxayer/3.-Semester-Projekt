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

        private Dictionary<IContextChannel, Session> activeClients;

        private DomainFacade()
        {
            activeClients = new Dictionary<IContextChannel, Session>();
        }

        public bool Connect(string tokenID)
        {
            if(activeClients.ContainsKey(OperationContext.Current.Channel))
            {
                return false;
            }

            string username = GeneralService.DomainFacade.Instance.UseToken(tokenID);
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            Session session = new Session(OperationContext.Current.Channel,
                OperationContext.Current.GetCallbackChannel<ICallback>(),
                username);

            activeClients.Add(OperationContext.Current.Channel, session);

            OnPlayerConnected();

            return true;
        }

        public bool Disconnect()
        {
            if (!activeClients.ContainsKey(OperationContext.Current.Channel))
            {
                return false;
            }

            activeClients.Remove(OperationContext.Current.Channel);
            return true;
        }

        private void OnPlayerConnected()
        {
            string username = activeClients[OperationContext.Current.Channel].Username;

            foreach(Session session in activeClients.Values)
            {
                if(session.Channel.Equals(OperationContext.Current.Channel))
                {
                    continue;
                }

                session.Callback.OnPlayerConnected(username);
            }
        }

        private class Session
        {
            public IContextChannel Channel { get; private set; }
            public ICallback Callback { get; private set; }
            public string Username { get; private set; }

            public Session(IContextChannel channel, ICallback callback, string username)
            {
                Channel = channel;
                Callback = callback;
                Username = username;
            }
        }

        public List<string> GetLobby()
        {
            List<string> lobby = new List<string>();

            foreach (Session session in activeClients.Values)
            {
                lobby.Add(session.Username);
            }

            return lobby;
        }
    }
}
