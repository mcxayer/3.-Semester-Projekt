using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MatchmakingWindow.xaml
    /// </summary>
    public partial class MatchmakingWindow : UserControl
    {
        MainWindow window;

        public MatchmakingWindow()
        {
            InitializeComponent();

            ServiceFacade.Instance.HandlePlayerMatchmade += OnPlayerMatchMade;
            ServiceFacade.Instance.HandlePlayerEnteredMatchmaking += OnPlayerEnteredMatchmaking;
            ServiceFacade.Instance.HandlePlayerExitedMatchmaking += OnPlayerExitedMatchmaking;
        }

        ~MatchmakingWindow()
        {
            try
            {
                if (ServiceFacade.Instance != null)
                {
                    ServiceFacade.Instance.HandlePlayerMatchmade -= OnPlayerMatchMade;
                    ServiceFacade.Instance.HandlePlayerEnteredMatchmaking -= OnPlayerEnteredMatchmaking;
                    ServiceFacade.Instance.HandlePlayerExitedMatchmaking -= OnPlayerExitedMatchmaking;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            window.GotoLobby();
        }

        private void OnPlayerMatchMade(string gameId)
        {
            // Get game here
            lblWaiting.Content = "Matchmade!!!!!!!!!!!!!!!!!!!!";
        }

        private void OnPlayerEnteredMatchmaking()
        {
            
        }

        private void OnPlayerExitedMatchmaking()
        {
            
        }
    }
}
