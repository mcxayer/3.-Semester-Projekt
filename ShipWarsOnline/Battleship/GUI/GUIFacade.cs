using Battleship.Game;
using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Battleship.GUI
{
    public class GUIFacade : IDisposable
    {
        private static readonly GUIFacade instance = new GUIFacade();
        public static GUIFacade Instance { get { return instance; } }

        private GUIWindowType currentWindowType;

        private IGUIWindow windowContainer;
        public IGUIWindow WindowContainer
        {
            private get { return windowContainer; }
            set { windowContainer = value; GotoWindow(GUIWindowType.MainMenu); }
        }

        private Dispatcher uiDispatcher;

        private GUIFacade()
        {
            GameContextFacade.Instance.HandlePlayerConnected += OnPlayerConnected;
            GameContextFacade.Instance.HandlePlayerDisconnected += OnPlayerDisconnected;
            GameContextFacade.Instance.HandlePlayerFailedConnecting += OnPlayerFailedConnecting;

            GameContextFacade.Instance.HandlePlayerEnteredMatchmaking += OnPlayerEnteredMatchmaking;
            GameContextFacade.Instance.HandlePlayerCancelledMatchmaking += OnPlayerCancelledMatchmaking;
            GameContextFacade.Instance.HandlePlayerMatchmade += OnPlayerMatchmade;

            GameContextFacade.Instance.HandleLobbyUpdated += OnLobbyUpdated;
            GameContextFacade.Instance.HandleGameInitialized += OnGameInit;
            GameContextFacade.Instance.HandleTurnTaken += OnTurnTaken;
            GameContextFacade.Instance.HandleShipDestroyed += OnShipDestroyed;
            GameContextFacade.Instance.HandlePlayerWon += OnPlayerWon;
            GameContextFacade.Instance.HandlePlayerLost += OnPlayerLost;

            GameContextFacade.Instance.HandlePlayerAccountCreated += OnPlayerAccountCreated;
            GameContextFacade.Instance.HandlePlayerAccountFailedCreation += OnPlayerAccountFailedCreation;

            uiDispatcher = Application.Current.Dispatcher;
        }

        public void Dispose()
        {
            try
            {
                GameContextFacade.Instance.HandlePlayerConnected -= OnPlayerConnected;
                GameContextFacade.Instance.HandlePlayerDisconnected -= OnPlayerDisconnected;
                GameContextFacade.Instance.HandlePlayerFailedConnecting -= OnPlayerFailedConnecting;

                GameContextFacade.Instance.HandlePlayerEnteredMatchmaking -= OnPlayerEnteredMatchmaking;
                GameContextFacade.Instance.HandlePlayerCancelledMatchmaking -= OnPlayerCancelledMatchmaking;
                GameContextFacade.Instance.HandlePlayerMatchmade -= OnPlayerMatchmade;

                GameContextFacade.Instance.HandleLobbyUpdated -= OnLobbyUpdated;
                GameContextFacade.Instance.HandleGameInitialized -= OnGameInit;
                GameContextFacade.Instance.HandleTurnTaken -= OnTurnTaken;
                GameContextFacade.Instance.HandleShipDestroyed -= OnShipDestroyed;
                GameContextFacade.Instance.HandlePlayerWon -= OnPlayerWon;
                GameContextFacade.Instance.HandlePlayerLost -= OnPlayerLost;

                GameContextFacade.Instance.HandlePlayerAccountCreated -= OnPlayerAccountCreated;
                GameContextFacade.Instance.HandlePlayerAccountFailedCreation -= OnPlayerAccountFailedCreation;
            }
            catch
            {
                // Nothing
            }
        }

        private void GotoWindow(GUIWindowType type)
        {
            currentWindowType = type;
            IGUIControl control = null;
            switch (type)
            {
                case GUIWindowType.MainMenu:
                    control = WindowContainer.GetMainMenuControl();
                    break;
                case GUIWindowType.Login:
                    control = WindowContainer.GetLoginControl();
                    break;
                case GUIWindowType.AccountCreation:
                    control = WindowContainer.GetAccountCreationControl();
                    break;
                case GUIWindowType.Lobby:
                    control = WindowContainer.GetLobbyControl();
                    break;
                case GUIWindowType.Matchmaking:
                    control = WindowContainer.GetMatchmakingControl();
                    break;
                case GUIWindowType.Game:
                    control = WindowContainer.GetGameControl();
                    break;

                default:
                    throw new System.Exception(string.Format("Window type {0} is not valid!", type));
            }

            if (control != null)
            {
                WindowContainer.SetDataContext(control.GetElement());
                control.OnSelected();
            }
        }

        public void GotoMainMenu()
        {
            if (currentWindowType != GUIWindowType.Login
                && currentWindowType != GUIWindowType.AccountCreation)
            {
                throw new Exception("Invalid control!");
            }

            GotoWindow(GUIWindowType.MainMenu);
        }

        public void GotoLogin()
        {
            if (currentWindowType != GUIWindowType.MainMenu
                && currentWindowType != GUIWindowType.AccountCreation)
            {
                throw new Exception("Invalid control!");
            }

            GotoWindow(GUIWindowType.Login);
        }

        public void GotoAccountCreation()
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                throw new Exception("Invalid control!");
            }

            GotoWindow(GUIWindowType.AccountCreation);
        }

        public void LoginAndConnectLobby(string username, string password)
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.LoginAndConnectLobby(username, password);
        }

        public void LogoutAndDisconnectLobby()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.LogoutAndDisconnectLobby();
        }

        public void Matchmake()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.Matchmake();
        }

        public void CancelMatchmaking()
        {
            if (currentWindowType != GUIWindowType.Matchmaking)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.CancelMatchmaking();
        }

        public void CreateAccount(string username, string password, string email)
        {
            if (currentWindowType != GUIWindowType.AccountCreation)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.CreateAccount(username, password, email);
        }

        public ReadOnly2DArray<ReadOnlySeaCell> GetPlayerCells()
        {
            return GameContextFacade.Instance.GetPlayerCells();
        }

        public ReadOnly2DArray<ReadOnlySeaCell> GetOpponentCells()
        {
            return GameContextFacade.Instance.GetOpponentCells();
        }

        public string GetPlayerName()
        {
            return GameContextFacade.Instance.GetPlayerName();
        }

        public string GetOpponentName()
        {
            return GameContextFacade.Instance.GetOpponentName();
        }

        public void TakeTurn(int x, int y)
        {
            if (currentWindowType != GUIWindowType.Game)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.TakeTurn(x, y);
        }

        public void UpdateLobby()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.UpdateLobby();
        }

        public bool IsPlayerTurn()
        {
            return GameContextFacade.Instance.IsPlayerTurn();
        }

        #region event callbacks
        /// <summary>
        /// Callbacks for all events invoked in GameContextFacade
        /// </summary>

        private void OnPlayerConnected()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Player connected to the game server!");

                if (currentWindowType != GUIWindowType.Login)
                {
                    return;
                }

                WindowContainer.GetLoginControl().OnPlayerConnected();
                GotoWindow(GUIWindowType.Lobby);
            }));
        }

        private void OnPlayerDisconnected()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Player disconnected from the game server!");

                GotoWindow(GUIWindowType.Login);
            }));
        }

        private void OnPlayerFailedConnecting()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Player failed to connect to the game server!");

                if (currentWindowType != GUIWindowType.Login)
                {
                    return;
                }

                WindowContainer.GetLoginControl().OnPlayerFailedConnecting();
            }));
        }

        private void OnPlayerEnteredMatchmaking()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Player entered matchmaking!");

                GotoWindow(GUIWindowType.Matchmaking);
            }));
        }

        private void OnPlayerCancelledMatchmaking()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Player exited matchmaking!");

                if (currentWindowType != GUIWindowType.Matchmaking)
                {
                    return;
                }

                WindowContainer.GetMatchmakingControl().OnPlayerCancelledMatchmaking();

                GotoWindow(GUIWindowType.Lobby);
            }));
        }

        private void OnPlayerMatchmade()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Matchmade to game!");

                if (currentWindowType != GUIWindowType.Matchmaking)
                {
                    return;
                }

                WindowContainer.GetMatchmakingControl().OnPlayerMatchmade();
            }));
        }

        private void OnLobbyUpdated(List<string> lobbyNames)
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Lobby updated!");

                if (currentWindowType != GUIWindowType.Lobby)
                {
                    return;
                }

                WindowContainer.GetLobbyControl().OnLobbyUpdated(lobbyNames);
            }));
        }

        private void OnGameInit()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                Console.WriteLine("Game has been initialized!");

                GotoWindow(GUIWindowType.Game);
                WindowContainer.GetGameControl().OnGameInit();
            }));
        }

        private void OnTurnTaken()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.Game)
                {
                    return;
                }

                WindowContainer.GetGameControl().OnTurnTaken();
            }));
        }

        private void OnShipDestroyed()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.Game)
                {
                    return;
                }

                WindowContainer.GetGameControl().OnShipDestroyed();
            }));
        }

        private void OnPlayerWon()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.Game)
                {
                    return;
                }

                WindowContainer.GetGameControl().OnPlayerWon();
            }));
        }

        private void OnPlayerLost()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.Game)
                {
                    return;
                }

                WindowContainer.GetGameControl().OnPlayerLost();
            }));
        }

        private void OnPlayerAccountCreated()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.AccountCreation)
                {
                    return;
                }

                WindowContainer.GetAccountCreationControl().OnPlayerAccountCreated();
            }));
        }

        private void OnPlayerAccountFailedCreation()
        {
            uiDispatcher.BeginInvoke(new Action(() =>
            {
                if (currentWindowType != GUIWindowType.AccountCreation)
                {
                    return;
                }

                WindowContainer.GetAccountCreationControl().OnPlayerAccountFailedCreation();
            }));
        }

        #endregion
    }
}
