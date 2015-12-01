using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            ServiceFacade.Instance.HandlePlayerConnected += OnPlayerConnected;
            ServiceFacade.Instance.HandlePlayerDisconnected += OnPlayerDisconnected;

            InitializeComponent();
            updateLobbyList();
        }

        ~LobbyWindow()
        {
            try
            {
                if (ServiceFacade.Instance != null)
                {
                    ServiceFacade.Instance.HandlePlayerConnected -= OnPlayerConnected;
                    ServiceFacade.Instance.HandlePlayerDisconnected -= OnPlayerDisconnected;
                }
            }
            catch
            {
                // Nothing
            }
        }

        private void OnPlayerConnected(string player)
        {
            updateLobbyList();
        }
        private void OnPlayerDisconnected(string player)
        {
            updateLobbyList();
        }

        public void updateLobbyList()
        {
            lstLobby.Items.Clear();
            List<string> lobby = ServiceFacade.Instance.GetLobby();

            foreach (string s in lobby)
            {
                List<string> usernames = new List<string>();
                usernames.Add(s);
                lstLobby.Items.Add(usernames);
            }
            
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void OnDisconnectButtonClicked(object sender, RoutedEventArgs e)
        {
            if (ServiceFacade.Instance.Disconnect())
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
            ServiceFacade.Instance.Matchmake();
            window.GotoMatchmaking();
        }

        private void OnUpdateButtonClicked(object sender, RoutedEventArgs e)
        {
            updateLobbyList();
        }
    }
}
