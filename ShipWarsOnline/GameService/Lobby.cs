using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class Lobby
    {
        private Dictionary<IContextChannel, Session> activeClients;
        private Queue<IContextChannel> matchmakingQueue;

        public Lobby()
        {
            activeClients = new Dictionary<IContextChannel, Session>();
            matchmakingQueue = new Queue<IContextChannel>();
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

        public bool Connect(string tokenID)
        {
            if (HasActiveClient(OperationContext.Current.Channel))
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

            session.Channel.Closed += OnChannelClosed;
            session.Channel.Faulted += OnChannelFaulted;

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

            OnPlayerDisconnected();

            Session session = activeClients[OperationContext.Current.Channel];
            session.Channel.Closed -= OnChannelClosed;
            session.Channel.Faulted -= OnChannelFaulted;

            activeClients.Remove(OperationContext.Current.Channel);
            return true;
        }

        public List<string> GetActiveClients()
        {
            List<string> lobby = new List<string>();

            foreach (Session session in activeClients.Values)
            {
                lobby.Add(session.Username);
            }

            return lobby;
        }

        public bool HasActiveClient(IContextChannel channel)
        {
            return activeClients.ContainsKey(OperationContext.Current.Channel);
        }

        public void Matchmake()
        {
            IContextChannel channel = OperationContext.Current.Channel;
            if (channel == null)
            {
                throw new NullReferenceException("Channel could not be found!");
            }

            if (matchmakingQueue.Contains(channel))
            {
                return;
            }

            Console.WriteLine("a");

            IContextChannel currentPlayer = OperationContext.Current.Channel;
            if (currentPlayer == null)
            {
                throw new NullReferenceException("Current player does not exist!");
            }

            Session currentPlayerSession = activeClients[currentPlayer];

            Console.WriteLine("b");

            if (matchmakingQueue.Count > 0)
            {
                IContextChannel otherPlayer = matchmakingQueue.Dequeue();
                if (currentPlayer == null)
                {
                    throw new NullReferenceException("Other player does not exist!");
                }

                Session otherPlayerSession = activeClients[otherPlayer];
                // Create game

                Console.WriteLine("c");

                string gameId = "MyRandomID";

                OnPlayerMatchmade(currentPlayerSession, gameId);
                OnPlayerMatchmade(otherPlayerSession, gameId);

                Console.WriteLine("d");

                return;
            }

            matchmakingQueue.Enqueue(OperationContext.Current.Channel);
            OnPlayerEnteredMatchmaking(currentPlayerSession);

            Console.WriteLine("e");
        }

        private void OnPlayerConnected()
        {
            // Check if client is not null
            string username = activeClients[OperationContext.Current.Channel].Username;

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(OperationContext.Current.Channel))
                {
                    continue;
                }

                session.Callback.OnPlayerConnected(username);
            }
        }

        private void OnPlayerDisconnected()
        {
            // Check if client is not null
            string username = activeClients[OperationContext.Current.Channel].Username;

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(OperationContext.Current.Channel))
                {
                    continue;
                }

                session.Callback.OnPlayerDisconnected(username);
            }
        }

        private void OnPlayerMatchmade(Session playerSession, string gameId)
        {
            if(playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            playerSession.Callback.OnPlayerMatchmade(gameId);
        }

        private void OnPlayerEnteredMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            playerSession.Callback.OnPlayerEnteredMatchmaking();
        }

        private void OnPlayerExitedMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            playerSession.Callback.OnPlayerExitedMatchmaking();
        }

        private void OnChannelClosed(object sender, EventArgs e)
        {
            OnPlayerDisconnected();
        }

        private void OnChannelFaulted(object sender, EventArgs e)
        {
            OnPlayerDisconnected();
        }
    }
}
