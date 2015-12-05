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
        private LinkedList<IContextChannel> matchmakingQueue;
        private Dictionary<IContextChannel, ServerGame> activeGames;

        public Lobby()
        {
            activeClients = new Dictionary<IContextChannel, Session>();
            matchmakingQueue = new LinkedList<IContextChannel>();
            activeGames = new Dictionary<IContextChannel, ServerGame>();
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

            activeClients.Add(OperationContext.Current.Channel, session);

            OnPlayerConnected(session);
            return true;
        }

        public bool Disconnect()
        {
            return ForceDisconnect(OperationContext.Current.Channel);
        }

        private bool ForceDisconnect(IContextChannel channel)
        {
            Session playerSession;
            if (!activeClients.TryGetValue(channel, out playerSession))
            {
                return false;
            }

            OnPlayerDisconnected(playerSession);
            playerSession.Channel.Closed -= OnChannelClosed;

            if(playerSession.state == SessionState.Matching)
            {
                ForceCancelMatchmaking(channel);
            }

            activeClients.Remove(channel);
            return true;
        }

        public List<string> GetLobbyClients()
        {
            List<string> lobby = new List<string>();

            foreach (Session session in activeClients.Values)
            {
                if(session.state == SessionState.InLobby)
                {
                    lobby.Add(session.Username);
                }
            }

            return lobby;
        }

        public bool HasActiveClient(IContextChannel channel)
        {
            return activeClients.ContainsKey(OperationContext.Current.Channel);
        }

        public void Matchmake()
        {
            IContextChannel playerChannel = OperationContext.Current.Channel;
            if (playerChannel == null)
            {
                throw new NullReferenceException("Current player does not exist!");
            }

            Session playerSession;
            if (!activeClients.TryGetValue(playerChannel, out playerSession))
            {
                return;
            }

            if (playerSession.state != SessionState.InLobby)
            {
                return;
            }

            playerSession.state = SessionState.Matching;

            if (matchmakingQueue.Count > 0)
            {
                OnPlayerEnteredMatchmaking(playerSession);

                IContextChannel otherPlayerChannel = matchmakingQueue.First.Value;
                matchmakingQueue.RemoveFirst();
                if (playerChannel == null)
                {
                    throw new NullReferenceException("Other player does not exist!");
                }

                Session otherPlayerSession;
                if (!activeClients.TryGetValue(otherPlayerChannel, out otherPlayerSession))
                {
                    throw new NullReferenceException("Current player is not connected!");
                }

                ServerGame game = new ServerGame(playerChannel, otherPlayerChannel);
                activeGames.Add(playerChannel, game);
                activeGames.Add(otherPlayerChannel, game);

                playerSession.state = SessionState.InGame;
                otherPlayerSession.state = SessionState.InGame;

                OnPlayerMatchmade(playerSession);
                OnPlayerMatchmade(otherPlayerSession);

                //ForceDisconnect(currentPlayer);
                //ForceDisconnect(otherPlayer);

                return;
            }

            matchmakingQueue.AddLast(playerSession.Channel);
            OnPlayerEnteredMatchmaking(playerSession);
        }

        public void CancelMatchmaking()
        {
            ForceCancelMatchmaking(OperationContext.Current.Channel);
        }

        private void ForceCancelMatchmaking(IContextChannel channel)
        {
            if (channel == null)
            {
                throw new NullReferenceException("Player does not exist!");
            }

            Session playerSession;
            if (!activeClients.TryGetValue(channel, out playerSession))
            {
                return;
            }

            if (playerSession.state != SessionState.Matching)
            {
                return;
            }

            playerSession.state = SessionState.InLobby;

            matchmakingQueue.Remove(channel);
            OnPlayerExitedMatchmaking(playerSession);
        }

        public void TakeTurn(int x, int y)
        {
            IContextChannel playerChannel = OperationContext.Current.Channel;
            if (playerChannel == null)
            {
                throw new NullReferenceException("Current player does not exist!");
            }

            ServerGame game;
            if (!activeGames.TryGetValue(playerChannel, out game))
            {
                throw new NullReferenceException("Current player is not in a game!");
            }

            game.TakeTurn(x, y, playerChannel);
        }

        public GameStateDTO GetGameState()
        {
            IContextChannel playerChannel = OperationContext.Current.Channel;
            if (playerChannel == null)
            {
                throw new NullReferenceException("Current player does not exist!");
            }

            ServerGame game;
            if (!activeGames.TryGetValue(playerChannel, out game))
            {
                throw new NullReferenceException("Current player is not in a game!");
            }

            return game.GetGameState(playerChannel);
        }

        private void OnPlayerConnected(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(playerSession.Channel))
                {
                    continue;
                }

                if(session.state != SessionState.InLobby 
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnPlayerConnected(playerSession.Username);
                session.Callback.OnLobbyUpdated();
            }
        }

        private void OnPlayerDisconnected(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(playerSession.Channel))
                {
                    continue;
                }

                if (session.state != SessionState.InLobby 
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnPlayerDisconnected(playerSession.Username);
                session.Callback.OnLobbyUpdated();
            }
        }

        private void OnPlayerMatchmade(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.Channel.State == CommunicationState.Opened)
            {
                playerSession.Callback.OnPlayerMatchmade();
            }
        }

        private void OnPlayerEnteredMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(playerSession.Channel))
                {
                    if (playerSession.Channel.State == CommunicationState.Opened)
                    {
                        playerSession.Callback.OnPlayerEnteredMatchmaking();
                    }
                    continue;
                }

                if (session.state != SessionState.InLobby 
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnLobbyUpdated();
            }
        }

        private void OnPlayerExitedMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            foreach (Session session in activeClients.Values)
            {
                if (session.Channel.Equals(playerSession.Channel))
                {
                    if(playerSession.Channel.State == CommunicationState.Opened)
                    {
                        playerSession.Callback.OnPlayerExitedMatchmaking();
                    }
                    continue;
                }

                if (session.state != SessionState.InLobby 
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnLobbyUpdated();
            }
        }

        private void OnChannelClosed(object sender, EventArgs e)
        {
            IContextChannel playerChannel = sender as IContextChannel;
            if (playerChannel == null)
            {
                throw new NullReferenceException("Could not get closed channel!");
            }

            ForceDisconnect(playerChannel);
        }

        private enum SessionState { InLobby, Matching, InGame }

        private class Session
        {
            public IContextChannel Channel { get; private set; }
            public ICallback Callback { get; private set; }
            public string Username { get; private set; }

            public SessionState state { get; set; }

            public Session(IContextChannel channel, ICallback callback, string username)
            {
                Channel = channel;
                Callback = callback;
                Username = username;
            }
        }
    }
}
