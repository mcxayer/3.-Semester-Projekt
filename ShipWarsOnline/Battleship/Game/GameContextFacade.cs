using GameServices;
using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
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
        public event Action<List<string>> HandleLobbyUpdated;
        public event Action HandleGameInitialized;
        public event Action HandleTurnTaken;
        public event Action HandleShipDestroyed;
        public event Action HandlePlayerWon;
        public event Action HandlePlayerLost;

        public event Action HandlePlayerAccountCreated;
        public event Action HandlePlayerAccountFailedCreation;

        private ClientGame clientGame;
        private ServiceFacade serviceFacade;

        private GameContextFacade()
        {
            serviceFacade = new ServiceFacade(new CallbackHandler(this));
        }

        #region general services

        public void Login(string username, string password)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.Login(username, password);
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to log in!");
                }
            });
        }

        public void Logout()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.Logout();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to log out!");
                }
            });
        }

        public void CreateAccount(string username, string password, string email)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.CreateAccount(username, password, email);
                    OnPlayerAccountCreated();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to create account!");
                    OnPlayerAccountFailedCreation();
                }
            });
        }

        #endregion

        #region game services

        public void LoginAndConnectLobby(string username, string password)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.Login(username, password);
                    serviceFacade.ConnectLobby();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to login and connect to lobby!");
                    OnPlayerFailedConnecting();
                }
            });
        }

        public void LogoutAndDisconnectLobby()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.DisconnectLobby();
                    serviceFacade.Logout();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to logout and disconnect from lobby!");
                }
            });
        }

        public void ConnectLobby()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.ConnectLobby();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to connect to lobby!");
                }
            });
        }

        public void DisconnectLobby()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.DisconnectLobby();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to disconnect to lobby!");
                }
            });
        }

        public void UpdateLobby()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    List<string> list = serviceFacade.GetLobby();
                    OnLobbyUpdated(list);
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to get lobby!");
                }
            });
        }

        public void Matchmake()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.Matchmake();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to matchmake!");
                }
            });
        }

        public void CancelMatchmaking()
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.CancelMatchmaking();
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to cancel matchmaking!");
                }
            });
        }

        public void TakeTurn(int x, int y)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    serviceFacade.TakeTurn(x, y);
                }
                catch (FaultException)
                {
                    Console.WriteLine("Failed to take turn!");
                }
            });
        }

        #endregion

        #region local methods

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

        public string GetPlayerName()
        {
            return clientGame.GetPlayerName();
        }

        public string GetOpponentName()
        {
            return clientGame.GetOpponentName();
        }

        public bool IsPlayerTurn()
        {
            return clientGame.IsPlayerTurn();
        }

        private void CreateClientGame(GameInitStateDTO initState)
        {
            clientGame = new ClientGame(initState);
        }

        #endregion

        #region event callbacks

        private void OnPlayerConnected()
        {
            if (HandlePlayerConnected != null)
            {
                HandlePlayerConnected();
            }
        }

        private void OnPlayerDisconnected()
        {
            if (HandlePlayerConnected != null)
            {
                HandlePlayerDisconnected();
            }
        }

        private void OnPlayerFailedConnecting()
        {
            if (HandlePlayerFailedConnecting != null)
            {
                HandlePlayerFailedConnecting();
            }
        }

        private void OnLobbyUpdated(List<string> lobbyNames)
        {
            if (HandleLobbyUpdated != null)
            {
                HandleLobbyUpdated(lobbyNames);
            }
        }

        private void OnPlayerMatchmade()
        {
            if (HandlePlayerMatchmade != null)
            {
                HandlePlayerMatchmade();
            }
        }

        private void OnPlayerEnteredMatchmaking()
        {
            if (HandlePlayerEnteredMatchmaking != null)
            {
                HandlePlayerEnteredMatchmaking();
            }
        }

        private void OnPlayerCancelledMatchmaking()
        {
            if (HandlePlayerCancelledMatchmaking != null)
            {
                HandlePlayerCancelledMatchmaking();
            }
        }

        private void OnGameInit(GameInitStateDTO initState)
        {
            CreateClientGame(initState);

            if (HandleGameInitialized != null)
            {
                HandleGameInitialized();
            }
        }

        private void OnTurnTaken(GameCellImpactDTO cellImpact)
        {
            if (cellImpact.PlayerIndex == clientGame.PlayerIndex)
            {
                clientGame.TakeTurn(cellImpact.AffectedPosX, cellImpact.AffectedPosY, cellImpact.Type);
            }
            else
            {
                clientGame.TakeTurn(cellImpact.AffectedPosX, cellImpact.AffectedPosY);
            }

            if (HandleTurnTaken != null)
            {
                HandleTurnTaken();
            }
        }

        private void OnShipDestroyed(GameShipDestroyedDTO shipDestroyed)
        {
            if (shipDestroyed.PlayerIndex == clientGame.PlayerIndex)
            {
                Bounds shipBounds = new Bounds(shipDestroyed.StartPosX,
                    shipDestroyed.EndPosY,
                    shipDestroyed.EndPosX,
                    shipDestroyed.StartPosY);

                clientGame.AddDestroyedShip(shipBounds);

                if(HandleShipDestroyed != null)
                {
                    HandleShipDestroyed();
                }
            }
        }

        private void OnPlayerWon(int playerIndex)
        {
            if (playerIndex == clientGame.PlayerIndex)
            {
                if (HandlePlayerWon != null)
                {
                    HandlePlayerWon();
                }
            }
            else
            {
                if (HandlePlayerLost != null)
                {
                    HandlePlayerLost();
                }
            }
        }

        private void OnPlayerAccountCreated()
        {
            if (HandlePlayerAccountCreated != null)
            {
                HandlePlayerAccountCreated();
            }
        }

        private void OnPlayerAccountFailedCreation()
        {
            if (HandlePlayerAccountFailedCreation != null)
            {
                HandlePlayerAccountFailedCreation();
            }
        }

        #endregion

        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
        private class CallbackHandler : GameServices.ICallback
        {
            private GameContextFacade facade;

            public CallbackHandler(GameContextFacade facade)
            {
                this.facade = facade;
            }

            public void OnPlayerConnected()
            {
                facade.OnPlayerConnected();
            }

            public void OnPlayerDisconnected()
            {
                facade.OnPlayerDisconnected();
            }

            public void OnPlayerFailedConnecting()
            {
                facade.OnPlayerFailedConnecting();
            }

            public void OnPlayerMatchmade()
            {
                facade.OnPlayerMatchmade();
            }

            public void OnPlayerEnteredMatchmaking()
            {
                facade.OnPlayerEnteredMatchmaking();
            }

            public void OnPlayerCancelledMatchmaking()
            {
                facade.OnPlayerCancelledMatchmaking();
            }

            public void OnLobbyUpdated()
            {
                facade.UpdateLobby();
            }

            public void OnGameInit(GameInitStateDTO initState)
            {
                facade.OnGameInit(initState);
            }

            public void OnTurnTaken(GameCellImpactDTO cellImpact)
            {
                facade.OnTurnTaken(cellImpact);
            }

            public void OnShipDestroyed(GameShipDestroyedDTO shipDestroyed)
            {
                facade.OnShipDestroyed(shipDestroyed);
            }

            public void OnPlayerWon(int playerIndex)
            {
                facade.OnPlayerWon(playerIndex);
            }
        }
    }
}
