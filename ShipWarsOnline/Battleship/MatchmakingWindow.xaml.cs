using System.Windows;
using System.Windows.Controls;

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

            GameContextFacade.Instance.HandlePlayerMatchmade += OnPlayerMatchMade;
            GameContextFacade.Instance.HandlePlayerExitedMatchmaking += OnPlayerExitedMatchmaking;
        }

        ~MatchmakingWindow()
        {
            try
            {
                if (GameContextFacade.Instance != null)
                {
                    GameContextFacade.Instance.HandlePlayerMatchmade -= OnPlayerMatchMade;
                    GameContextFacade.Instance.HandlePlayerExitedMatchmaking -= OnPlayerExitedMatchmaking;
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
            GameContextFacade.Instance.CancelMatchmaking();
        }

        private void OnPlayerMatchMade()
        {
            window.GotoGame();
        }

        private void OnPlayerExitedMatchmaking()
        {
            window.GotoLobby();
        }
    }
}
