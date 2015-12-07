﻿using GameService;
using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Battleship
{
    public class GameContextFacade
    {
        private static readonly GameContextFacade instance = new GameContextFacade();
        public static GameContextFacade Instance { get { return instance; } }

        public event Action<string> HandlePlayerConnected;
        public event Action<string> HandlePlayerDisconnected;
        public event Action HandlePlayerMatchmade;
        public event Action HandlePlayerEnteredMatchmaking;
        public event Action HandlePlayerExitedMatchmaking;
        public event Action HandleLobbyUpdated;

        private int playerIndex;
        private IGame game;
        private ServiceFacade serviceFacade;
        private Dispatcher uiDispatcher;

        private GameContextFacade()
        {
            serviceFacade = new ServiceFacade(new CallbackHandler(this));

            uiDispatcher = Dispatcher.CurrentDispatcher;
        }

        #region general services

        public string Login(string username, string password)
        {
            return uiDispatcher.Invoke(new Func<string>(() => serviceFacade.Login(username, password)));
        }

        public void Logout(string tokenId)
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.Logout(tokenId)));
        }

        public void CreateAccount(string username, string password, string email)
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.CreateAccount(username, password, email)));
        }

        #endregion

        #region game services

        public List<string> GetLobby()
        {
            return uiDispatcher.Invoke(new Func<List<string>>(() => serviceFacade.GetLobby()));
        }

        public bool Connect(string tokenId)
        {
            return uiDispatcher.Invoke(new Func<bool>(() => serviceFacade.Connect(tokenId)));
        }

        public bool Disconnect()
        {
            return uiDispatcher.Invoke(new Func<bool>(() => serviceFacade.Disconnect()));
        }

        public void Matchmake()
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.Matchmake()));
        }

        public void CancelMatchmaking()
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.CancelMatchmaking()));
        }

        #endregion

        public void CreateNetworkedGame()
        {
            GameStateDTO state = serviceFacade.GetGameState();
            playerIndex = state.PlayerIndex;

            game = new NetworkedGame(state);
        }

        public void TakeTurn(int x, int y)
        {
            if(game == null)
            {
                throw new NullReferenceException("No game exists!");
            }

            game.TakeTurn(x, y);
        }

        public ReadOnly2DArray<ReadOnlySeaCell> GetPlayerCells()
        {
            if (game == null)
            {
                throw new NullReferenceException("No game exists!");
            }

            return game.ReadOnlyGrids[playerIndex].ReadOnlyCells;
        } 

        public ReadOnly2DArray<ReadOnlySeaCell> GetOpponentCells()
        {
            if (game == null)
            {
                throw new NullReferenceException("No game exists!");
            }

            return game.ReadOnlyGrids[(playerIndex + 1) % 2].ReadOnlyCells;
        }

        //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
        private class CallbackHandler : GameService.ICallback
        {
            private GameContextFacade facade;

            public CallbackHandler(GameContextFacade facade)
            {
                this.facade = facade;
            }

            public void OnPlayerConnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} connected to the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerConnected(player)));
                }
            }

            public void OnPlayerDisconnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} disconnected from the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerDisconnected(player)));
                }
            }

            public void OnPlayerMatchmade()
            {
                Console.WriteLine("Matchmade to game!");
                if (facade.HandlePlayerMatchmade != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerMatchmade()));
                }
            }

            public void OnPlayerEnteredMatchmaking()
            {
                Console.WriteLine("Player entered matchmaking!");
                if (facade.HandlePlayerEnteredMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerEnteredMatchmaking()));
                }
            }

            public void OnPlayerExitedMatchmaking()
            {
                Console.WriteLine("Player exited matchmaking!");
                if (facade.HandlePlayerExitedMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerExitedMatchmaking()));
                }
            }

            public void OnLobbyUpdated()
            {
                if (facade.HandleLobbyUpdated != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandleLobbyUpdated()));
                }
            }

            public void OnGameUpdated(GameDeltaStateDTO deltaState)
            {
                throw new NotImplementedException();
            }
        }
    }
}
