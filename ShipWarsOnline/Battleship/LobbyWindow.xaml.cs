using Battleship.Game;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyWindow : UserControl
    {
        private MainWindow window;

        public LobbyWindow()
        {
            GameContextFacade.Instance.HandleLobbyUpdated += OnLobbyUpdated;

            InitializeComponent();
            UpdateLobbyList();
        }

        ~LobbyWindow()
        {
            try
            {
                if (GameContextFacade.Instance != null)
                {
                    GameContextFacade.Instance.HandleLobbyUpdated -= OnLobbyUpdated;
                }
            }
            catch
            {
                // Nothing
            }
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        public void UpdateLobbyList()
        {
            lstLobby.Items.Clear();
            List<string> lobby = GameContextFacade.Instance.GetLobby();

            foreach (string s in lobby)
            {
                List<string> usernames = new List<string>();
                usernames.Add(s);
                lstLobby.Items.Add(usernames);
            }
            
        }

        private void OnLobbyUpdated()
        {
            Console.WriteLine("Lobby updated!");
            UpdateLobbyList();
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
            if (GameContextFacade.Instance.Disconnect())
            {
                // Der skal nok også kaldes logout, så tokenID bliver expired
                window.GotoMainMenu();
            }
            else
            {
                lblInfo.Content = "Error disconnecting";
            }
        }

        private void OnMatchButtonClicked(object sender, RoutedEventArgs e)
        {
            GameContextFacade.Instance.Matchmake();
            window.GotoMatchmaking();
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            UpdateLobbyList();
        }
    }
}
