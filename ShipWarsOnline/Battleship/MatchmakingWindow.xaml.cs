using Battleship.Game;
using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MatchmakingWindow.xaml
    /// </summary>
    public partial class MatchmakingWindow : UserControl
    {
        public MatchmakingWindow()
        {
            InitializeComponent();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.CancelMatchmaking();
        }
    }
}
