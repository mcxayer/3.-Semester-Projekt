using SecurityTokenServices;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GameServices
{
    public class Lobby
    {
        private Dictionary<IContextChannel, Session> activeClients;
        private LinkedList<IContextChannel> matchmakingQueue;
        private Dictionary<IContextChannel, ServerGame> activeGames;

        private ISecurityTokenService tokenService;

        public Lobby()
        {
            activeClients = new Dictionary<IContextChannel, Session>();
            matchmakingQueue = new LinkedList<IContextChannel>();
            activeGames = new Dictionary<IContextChannel, ServerGame>();

            var tokenFactory = new ChannelFactory<ISecurityTokenService>("TokenServiceEndpoint");
            tokenService = tokenFactory.CreateChannel();
        }

        public void Connect(string tokenID)
        {
            if (HasActiveClient(OperationContext.Current.Channel))
            {
                throw new Exception("Player already connected!");
            }

            string username = tokenService.UseToken(tokenID);
            if (string.IsNullOrEmpty(username))
            {
                OnPlayerFailedConnecting(OperationContext.Current.GetCallbackChannel<IGameServiceCallback>());
                return;
            }

            Session session = new Session(OperationContext.Current.Channel,
                OperationContext.Current.GetCallbackChannel<IGameServiceCallback>(),
                username);

            session.Channel.Closed += OnChannelClosed;

            activeClients.Add(OperationContext.Current.Channel, session);

            OnPlayerConnected(session);
        }

        public void Disconnect()
        {
            ForceDisconnect(OperationContext.Current.Channel);
        }

        private void ForceDisconnect(IContextChannel playerChannel)
        {
            Session playerSession;
            if (!activeClients.TryGetValue(playerChannel, out playerSession))
            {
                throw new Exception("Player not connected!");
            }

            OnPlayerDisconnected(playerSession);
            playerSession.Channel.Closed -= OnChannelClosed;

            if (playerSession.State == SessionState.Matching)
            {
                ForceCancelMatchmaking(playerChannel);
            }

            activeClients.Remove(playerChannel);
        }

        public List<string> GetLobbyClients()
        {
            List<string> lobby = new List<string>();

            foreach (Session session in activeClients.Values)
            {
                if (session.State == SessionState.InLobby)
                {
                    lobby.Add(session.Username);
                }
            }

            return lobby;
        }

        private bool HasActiveClient(IContextChannel playerChannel)
        {
            return activeClients.ContainsKey(playerChannel);
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

            if (playerSession.State != SessionState.InLobby)
            {
                return;
            }

            playerSession.State = SessionState.Matching;

            if (matchmakingQueue.Count > 0)
            {
                OnPlayerEnteredMatchmaking(playerSession);

                IContextChannel otherPlayerChannel = matchmakingQueue.First.Value;
                matchmakingQueue.RemoveFirst();
                if (otherPlayerChannel == null)
                {
                    throw new NullReferenceException("Other player does not exist!");
                }

                Session otherPlayerSession;
                if (!activeClients.TryGetValue(otherPlayerChannel, out otherPlayerSession))
                {
                    throw new NullReferenceException("Other player is not connected!");
                }

                OnPlayerMatchmade(playerSession);
                OnPlayerMatchmade(otherPlayerSession);

                CreateNetworkGame(playerSession, otherPlayerSession);

                return;
            }

            matchmakingQueue.AddLast(playerSession.Channel);
            OnPlayerEnteredMatchmaking(playerSession);
        }

        public void CancelMatchmaking()
        {
            ForceCancelMatchmaking(OperationContext.Current.Channel);
        }

        private void ForceCancelMatchmaking(IContextChannel playerChannel)
        {
            if (playerChannel == null)
            {
                throw new NullReferenceException("Player does not exist!");
            }

            Session playerSession;
            if (!activeClients.TryGetValue(playerChannel, out playerSession))
            {
                return;
            }

            if (playerSession.State != SessionState.Matching)
            {
                return;
            }

            playerSession.State = SessionState.InLobby;

            matchmakingQueue.Remove(playerChannel);
            OnPlayerCancelledMatchmaking(playerSession);
        }

        private void CreateNetworkGame(Session player1Session, Session player2Session)
        {
            if (player1Session == null || player2Session == null)
            {
                throw new NullReferenceException("A player does not exist!");
            }

            if (player1Session.State != SessionState.Matching
                || player2Session.State != SessionState.Matching)
            {
                throw new Exception("A player is not in matchmaking!");
            }

            if (player1Session.Channel.State != CommunicationState.Opened
                || player2Session.Channel.State != CommunicationState.Opened)
            {
                ForceCancelMatchmaking(player1Session.Channel);
                ForceCancelMatchmaking(player2Session.Channel);
                return;
            }

            ServerGame game = new ServerGame(player1Session.Channel, player2Session.Channel);

            activeGames.Add(player1Session.Channel, game);
            activeGames.Add(player2Session.Channel, game);

            player1Session.State = SessionState.InGame;
            player2Session.State = SessionState.InGame;

            GameInitStateDTO initStatePlayer1 = game.GetInitGameState(player1Session.Channel);
            initStatePlayer1.PlayerName = player1Session.Username;
            initStatePlayer1.OpponentName = player2Session.Username;
            OnGameInitialized(player1Session, initStatePlayer1);

            GameInitStateDTO initStatePlayer2 = game.GetInitGameState(player2Session.Channel);
            initStatePlayer2.PlayerName = player2Session.Username;
            initStatePlayer2.OpponentName = player1Session.Username;
            OnGameInitialized(player2Session, initStatePlayer2);
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

            OnTurnTaken(game);
        }

        private void OnGameInitialized(Session playerSession, GameInitStateDTO initState)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.State != SessionState.InGame)
            {
                throw new Exception("Player is not in game!");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                return;
            }

            playerSession.Callback.OnGameInit(initState);
        }

        private void OnPlayerConnected(Session playerSession)
        {
            if (playerSession == null)
            {
                throw new ArgumentNullException("playerSession");
            }

            if (playerSession.State != SessionState.InLobby)
            {
                throw new Exception("Player is not in lobby!");
            }

            if (playerSession.Channel.State != CommunicationState.Opened)
            {
                ForceDisconnect(playerSession.Channel);
                return;
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

            OnLobbyUpdated();

            if (playerSession.Channel.State == CommunicationState.Closed
                || playerSession.Channel.State == CommunicationState.Faulted)
            {
                return;
            }

            playerSession.Callback.OnPlayerDisconnected();
        }

        private void OnPlayerFailedConnecting(IGameServiceCallback playerCallback)
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
                return;
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
                ForceCancelMatchmaking(playerSession.Channel);
                return;
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
                return;
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
                if (session.State != SessionState.InLobby
                    || session.Channel.State != CommunicationState.Opened)
                {
                    continue;
                }

                session.Callback.OnLobbyUpdated();
            }
        }

        private void OnTurnTaken(ServerGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            foreach (IContextChannel channel in game.ReadOnlyPlayerChannels)
            {
                Session playerSession = GetSession(channel);
                if (playerSession == null)
                {
                    throw new ArgumentNullException("playerSession");
                }

                if (playerSession.State != SessionState.InGame)
                {
                    throw new Exception("A player is not in game!");
                }

                if (playerSession.Channel.State != CommunicationState.Opened)
                {
                    throw new CommunicationException("Player connection is not open!");
                }

                playerSession.Callback.OnTurnTaken(game.GetCellImpact());

                GameShipDestroyedDTO shipDestroyed = game.GetDestroyedShip();
                if (shipDestroyed != null)
                {
                    playerSession.Callback.OnShipDestroyed(shipDestroyed);
                }

                if(game.IsGameOver())
                {
                    playerSession.Callback.OnPlayerWon(game.GetWinner());
                    playerSession.State = SessionState.PostGame;
                }
            }
        }

        private Session GetSession(IContextChannel playerChannel)
        {
            Session session;
            if (activeClients.TryGetValue(playerChannel, out session))
            {
                return session;
            }

            return null;
        }

        private enum SessionState { InLobby, Matching, InGame, PostGame }

        private class Session
        {
            public IContextChannel Channel { get; private set; }
            public IGameServiceCallback Callback { get; private set; }
            public string Username { get; private set; }

            public SessionState State { get; set; }

            public Session(IContextChannel channel, IGameServiceCallback callback, string username)
            {
                Channel = channel;
                Callback = callback;
                Username = username;
            }
        }
    }
}
