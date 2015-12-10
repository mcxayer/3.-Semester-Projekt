using Battleship.Game;
using Battleship.GUI;
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
        public LobbyWindow()
        {
            InitializeComponent();
            UpdateLobbyList();
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

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
            GameContextFacade.Instance.Disconnect();
            GameContextFacade.Instance.Logout();

            //if (GameContextFacade.Instance.Disconnect())
            //{
            //    // Der skal nok også kaldes logout, så tokenID bliver expired
            //    window.GotoMainMenu();
            //}
            //else
            //{
            //    lblInfo.Content = "Error disconnecting";
            //}
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            UpdateLobbyList();
        }

        private void OnFindMatchButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.Matchmake();
        }
    }
}
