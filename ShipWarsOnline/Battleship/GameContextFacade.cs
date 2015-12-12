using GameService;
using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows.Threading;

namespace Battleship.Game
{
    public class GameContextFacade
    {
        private static readonly GameContextFacade instance = new GameContextFacade();
        public static GameContextFacade Instance { get { return instance; } }

        public event Action HandlePlayerConnected;
        public event Action HandlePlayerDisconnected;
        public event Action HandlePlayerFailedConnecting;
        public event Action HandlePlayerMatchmade;
        public event Action HandlePlayerEnteredMatchmaking;
        public event Action HandlePlayerCancelledMatchmaking;
        public event Action HandleLobbyUpdated;
        public event Action HandleGameInitialized;
        public event Action HandleTurnTaken;
        public event Action HandlePlayerWon;
        public event Action HandlePlayerLost;

        private ClientGame clientGame;
        private ServiceFacade serviceFacade;
        private Dispatcher uiDispatcher;

        private GameContextFacade()
        {
            serviceFacade = new ServiceFacade(new CallbackHandler(this));

            uiDispatcher = Dispatcher.CurrentDispatcher;
        }

        #region general services

        public bool Login(string username, string password)
        {
            return uiDispatcher.Invoke(new Func<bool>(() => serviceFacade.Login(username, password)));
        }

        public void Logout()
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.Logout()));
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

        public void Connect()
        {
            uiDispatcher.Invoke(new Action(() => serviceFacade.Connect()));
        }

        public void Disconnect()
        {
            uiDispatcher.Invoke(new Action(() => serviceFacade.Disconnect()));
        }

        public void Matchmake()
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.Matchmake()));
        }

        public void CancelMatchmaking()
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.CancelMatchmaking()));
        }

        public void TakeTurn(int x, int y)
        {
            uiDispatcher.BeginInvoke(new Action(() => serviceFacade.TakeTurn(x, y)));
        }

        #endregion

        public ReadOnly2DArray<ReadOnlySeaCell> GetPlayerCells()
        {
            if (clientGame == null)
            {
                throw new NullReferenceException("No game exists!");
            }

            return clientGame.ReadOnlyGrids[clientGame.PlayerIndex].ReadOnlyCells;
        } 

        public ReadOnly2DArray<ReadOnlySeaCell> GetOpponentCells()
        {
            if (clientGame == null)
            {
                throw new NullReferenceException("No game exists!");
            }

            return clientGame.ReadOnlyGrids[(clientGame.PlayerIndex + 1) % 2].ReadOnlyCells;
        }

        public bool IsPlayerTurn()
        {
            return clientGame.IsPlayerTurn();
        }

        private void CreateClientGame(GameInitStateDTO initState)
        {
            clientGame = new ClientGame(initState);
        }

        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
        private class CallbackHandler : GameService.ICallback
        {
            private GameContextFacade facade;

            public CallbackHandler(GameContextFacade facade)
            {
                this.facade = facade;
            }

            public void OnPlayerConnected()
            {
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerConnected()));
                }
            }

            public void OnPlayerDisconnected()
            {
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerDisconnected()));
                }
            }

            public void OnPlayerFailedConnecting()
            {
                if (facade.HandlePlayerFailedConnecting != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerFailedConnecting()));
                }
            }

            public void OnPlayerMatchmade()
            {
                if (facade.HandlePlayerMatchmade != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerMatchmade()));
                }
            }

            public void OnPlayerEnteredMatchmaking()
            {
                if (facade.HandlePlayerEnteredMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerEnteredMatchmaking()));
                }
            }

            public void OnPlayerCancelledMatchmaking()
            {
                if (facade.HandlePlayerCancelledMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerCancelledMatchmaking()));
                }
            }

            public void OnLobbyUpdated()
            {
                if (facade.HandleLobbyUpdated != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandleLobbyUpdated()));
                }
            }

            public void OnGameInit(GameInitStateDTO initState)
            {
                facade.CreateClientGame(initState);

                if (facade.HandleGameInitialized != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandleGameInitialized()));
                }
            }

            public void OnTurnTaken(GameCellImpactDTO cellImpact)
            {
                if(cellImpact.PlayerIndex == facade.clientGame.PlayerIndex)
                {
                    facade.clientGame.TakeTurn(cellImpact.AffectedPosX, cellImpact.AffectedPosY, cellImpact.Type);
                }
                else
                {
                    facade.clientGame.TakeTurn(cellImpact.AffectedPosX, cellImpact.AffectedPosY);
                }

                if (facade.HandleTurnTaken != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandleTurnTaken()));
                }
            }

            public void OnShipDestroyed(GameShipDestroyedDTO shipDestroyed)
            {
                if (shipDestroyed.PlayerIndex == facade.clientGame.PlayerIndex)
                {
                    Bounds shipBounds = new Bounds(shipDestroyed.StartPosX,
                        shipDestroyed.EndPosY,
                        shipDestroyed.EndPosX,
                        shipDestroyed.StartPosY);

                    facade.clientGame.AddDestroyedShip(shipBounds);
                }
            }

            public void OnPlayerWon(int playerIndex)
            {
                if(playerIndex == facade.clientGame.PlayerIndex)
                {
                    if (facade.HandlePlayerWon != null)
                    {
                        facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerWon()));
                    }
                }
                else
                {
                    if (facade.HandlePlayerLost != null)
                    {
                        facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerLost()));
                    }
                }
            }
        }
    }
}
