using Battleship.Game;
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

        private IGUIController guiController;
        public IGUIController GUIController { set { guiController = value; GotoWindow(GUIWindowType.MainMenu); } }

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

        public bool GotoMainMenu()
        {
            if (currentWindowType != GUIWindowType.Login
                && currentWindowType != GUIWindowType.AccountCreation)
            {
                return false;
            }

            GotoWindow(GUIWindowType.MainMenu);
            return true;
        }

        public bool GotoLogin()
        {
            if(currentWindowType != GUIWindowType.MainMenu
                && currentWindowType != GUIWindowType.AccountCreation)
            {
                return false;
            }

            GotoWindow(GUIWindowType.Login);
            return true;
        }

        public bool GotoAccountCreation()
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                return false;
            }

            GotoWindow(GUIWindowType.AccountCreation);
            return true;
        }

        public bool Login(string username, string password)
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                return false;
            }

            GameContextFacade.Instance.Login(username, password);
            return true;
        }

        public bool Connect()
        {
            if (currentWindowType != GUIWindowType.Login)
            {
                return false;
            }

            GameContextFacade.Instance.Connect();
            return true;
        }

        public bool Matchmake()
        {
            if (currentWindowType != GUIWindowType.Lobby)
            {
                return false;
            }

            GameContextFacade.Instance.Matchmake();
            return true;
        }

        public bool CancelMatchmaking()
        {
            if (currentWindowType != GUIWindowType.Matchmaking)
            {
                return false;
            }

            GameContextFacade.Instance.CancelMatchmaking();
            return true;
        }

        public bool CreateAccount(string username, string password, string email)
        {
            if (currentWindowType != GUIWindowType.AccountCreation)
            {
                return false;
            }

            GameContextFacade.Instance.CreateAccount(username, password, email);
            return true;
        }

        private void GotoWindow(GUIWindowType type)
        {
            currentWindowType = type;
            guiController.GotoWindow(type);
        }

        private void OnPlayerConnected()
        {
            Console.WriteLine("Player connected to the game server!");

            if(guiController == null)
            {
                return;
            }

            GotoWindow(GUIWindowType.Lobby);
        }

        private void OnPlayerDisconnected()
        {
            Console.WriteLine("Player disconnected from the game server!");

            if (guiController == null)
            {
                return;
            }

            GotoWindow(GUIWindowType.Login);
        }

        private void OnPlayerFailedConnecting()
        {
            Console.WriteLine("Player failed to connect to the game server!");

            if (guiController == null)
            {
                return;
            }

            //TODO
        }

        private void OnPlayerEnteredMatchmaking()
        {
            Console.WriteLine("Player entered matchmaking!");

            if (guiController == null)
            {
                return;
            }

            GotoWindow(GUIWindowType.Matchmaking);
        }

        private void OnPlayerCancelledMatchmaking()
        {
            Console.WriteLine("Player exited matchmaking!");

            if (guiController == null)
            {
                return;
            }

            GotoWindow(GUIWindowType.Lobby);
        }

        private void OnPlayerMatchmade()
        {
            Console.WriteLine("Matchmade to game!");

            if (guiController == null)
            {
                return;
            }

            // TODO: Prepare to be matched
        }

        private void OnLobbyUpdated()
        {
            Console.WriteLine("Lobby updated!");

            if (guiController == null)
            {
                return;
            }

            // TODO
        }

        private void OnGameInit()
        {
            Console.WriteLine("Game has been initialized!");

            if (guiController == null)
            {
                return;
            }

            GotoWindow(GUIWindowType.Game);
        }
    }
}
