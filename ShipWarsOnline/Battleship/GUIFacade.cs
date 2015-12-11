using Battleship.Game;
using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

            if(control != null)
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
            if(currentWindowType != GUIWindowType.MainMenu
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

        public void Login(string username, string password)
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.Login(username, password);
        }

        public void Logout()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.Logout();
        }

        public void Connect()
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.Connect();
        }

        public void Disconnect()
        {
            if (currentWindowType != GUIWindowType.Lobby
                && currentWindowType != GUIWindowType.Game)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.Disconnect();
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

        public void TakeTurn(int x, int y)
        {
            if (currentWindowType != GUIWindowType.Game)
            {
                throw new Exception("Invalid control!");
            }

            GameContextFacade.Instance.TakeTurn(x, y);
        }

        public List<string> GetLobby()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                throw new Exception("Invalid control!");
            }

            return GameContextFacade.Instance.GetLobby();
        }

        private void OnPlayerConnected()
        {
            Console.WriteLine("Player connected to the game server!");

            GotoWindow(GUIWindowType.Lobby);
        }

        private void OnPlayerDisconnected()
        {
            Console.WriteLine("Player disconnected from the game server!");

            GotoWindow(GUIWindowType.Login);
        }

        private void OnPlayerFailedConnecting()
        {
            Console.WriteLine("Player failed to connect to the game server!");

            if(currentWindowType != GUIWindowType.Login)
            {
                return;
            }

            WindowContainer.GetLoginControl().OnPlayerFailedConnecting();
        }

        private void OnPlayerEnteredMatchmaking()
        {
            Console.WriteLine("Player entered matchmaking!");

            GotoWindow(GUIWindowType.Matchmaking);
        }

        private void OnPlayerCancelledMatchmaking()
        {
            Console.WriteLine("Player exited matchmaking!");

            GotoWindow(GUIWindowType.Lobby);
        }

        private void OnPlayerMatchmade()
        {
            Console.WriteLine("Matchmade to game!");

            if (currentWindowType != GUIWindowType.Matchmaking)
            {
                return;
            }

            WindowContainer.GetMatchmakingControl().OnPlayerMatchmade();
        }

        private void OnLobbyUpdated()
        {
            Console.WriteLine("Lobby updated!");

            if (currentWindowType != GUIWindowType.Lobby)
            {
                return;
            }

            WindowContainer.GetLobbyControl().OnLobbyUpdated();
        }

        private void OnGameInit()
        {
            Console.WriteLine("Game has been initialized!");

            GotoWindow(GUIWindowType.Game);
            WindowContainer.GetGameControl().OnGameInit();
        }
    }
}
