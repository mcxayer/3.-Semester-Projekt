using System;
using System.Collections.Generic;
using System.ServiceModel;

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

        public void Connect(string tokenID)
        {
            if (HasActiveClient(OperationContext.Current.Channel))
            {
                throw new Exception("Player already connected!");
            }

            string username = GeneralService.DomainFacade.Instance.UseToken(tokenID);
            if (string.IsNullOrEmpty(username))
            {
                OnPlayerFailedConnecting(OperationContext.Current.GetCallbackChannel<ICallback>());
                return;
            }

            Session session = new Session(OperationContext.Current.Channel,
                OperationContext.Current.GetCallbackChannel<ICallback>(),
                username);

            session.Channel.Closed += OnChannelClosed;

            activeClients.Add(OperationContext.Current.Channel, session);

            OnPlayerConnected(session);
        }

        public void Disconnect()
        {
            ForceDisconnect(OperationContext.Current.Channel);
        }

        private void ForceDisconnect(IContextChannel channel)
        {
            Session playerSession;
            if (!activeClients.TryGetValue(channel, out playerSession))
            {
                throw new Exception("Player not connected!");
            }

            OnPlayerDisconnected(playerSession);
            playerSession.Channel.Closed -= OnChannelClosed;

            if(playerSession.state == SessionState.Matching)
            {
                ForceCancelMatchmaking(channel);
            }

            activeClients.Remove(channel);
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

                OnPlayerMatchmade(playerSession);
                OnPlayerMatchmade(otherPlayerSession);

                CreateNetworkGame(playerSession, otherPlayerSession);

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
            OnPlayerCancelledMatchmaking(playerSession);
        }

        private void CreateNetworkGame(Session player1Session, Session player2Session)
        {
            if (player1Session == null || player2Session == null)
            {
                throw new NullReferenceException("A player does not exist!");
            }

            if (player1Session.state != SessionState.Matching
                || player2Session.state != SessionState.Matching)
            {
                throw new Exception("A player is not in matchmaking!");
            }

            if(player1Session.Channel.State != CommunicationState.Opened
                || player2Session.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("A player connection is not open!");
            }

            ServerGame game = new ServerGame(player1Session.Channel, player2Session.Channel);
            activeGames.Add(player1Session.Channel, game);
            activeGames.Add(player2Session.Channel, game);

            player1Session.state = SessionState.InGame;
            player2Session.state = SessionState.InGame;

            OnGameInitialized(player1Session, game.GetInitGameState(player1Session.Channel));
            OnGameInitialized(player2Session, game.GetInitGameState(player2Session.Channel));
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

        private void OnGameInitialized(Session playerSession, GameInitStateDTO initState)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.state != SessionState.InGame)
            {
                throw new Exception("Player is not in game!");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnGameInit(initState);
        }

        private void OnPlayerConnected(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.state != SessionState.InLobby)
            {
                throw new Exception("Player is not in lobby!");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnPlayerConnected();
            OnLobbyUpdated();
        }

        private void OnPlayerDisconnected(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnPlayerDisconnected();
            OnLobbyUpdated();
        }

        private void OnPlayerFailedConnecting(ICallback playerCallback)
        {
            if (playerCallback == null)
            {
                throw new ArgumentNullException("playerCallback");
            }

            playerCallback.OnPlayerFailedConnecting();
        }

        private void OnPlayerMatchmade(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnPlayerMatchmade();
        }

        private void OnPlayerEnteredMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnPlayerEnteredMatchmaking();
            OnLobbyUpdated();
        }

        private void OnPlayerCancelledMatchmaking(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                throw new CommunicationException("Player connection is not open!");
            }

            playerSession.Callback.OnPlayerCancelledMatchmaking();
            OnLobbyUpdated();
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

        private void OnLobbyUpdated()
        {
            foreach (Session session in activeClients.Values)
            {
                if (session.state != SessionState.InLobby
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnLobbyUpdated();
            }
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
