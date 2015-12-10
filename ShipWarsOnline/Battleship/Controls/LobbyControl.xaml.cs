using Battleship.Game;
using Battleship.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LobbyWindow.xaml
    /// </summary>
    public partial class LobbyControl : UserControl, IGUILobby, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> lobbyList;
        public List<string> LobbyList
        {
            get { return lobbyList; }
            private set { lobbyList = value; OnPropertyChanged("LobbyList"); }
        }

        public LobbyControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void UpdateLobbyList()
        {
            LobbyList = GUIFacade.Instance.GetLobby();
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.Disconnect();
            GUIFacade.Instance.Logout();

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

        public void OnLobbyUpdated()
        {
            UpdateLobbyList();
        }

        public void OnSelected()
        {
            UpdateLobbyList();
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
