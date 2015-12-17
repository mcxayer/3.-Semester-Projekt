﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Client.GUI.Controls
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
            GUIFacade.Instance.UpdateLobby();
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
            EnableControl(false);
            GUIFacade.Instance.LogoutAndDisconnectLobby();
        }

        private void OnFindMatchButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.Matchmake();
        }

        public void OnLobbyUpdated(List<string> lobbyNames)
        {
            LobbyList = lobbyNames;
        }

        public void OnSelected()
        {
            EnableControl(true);
            UpdateLobbyList();
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        private void EnableControl(bool enable)
        {
            gridContent.IsEnabled = enable;
            gridFooter.IsEnabled = enable;
        }
    }
}