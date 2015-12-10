using Battleship.Game;
using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MatchmakingWindow.xaml
    /// </summary>
    public partial class MatchmakingControl : UserControl, IGUIMatchmaking
    {
        public MatchmakingControl()
        {
            InitializeComponent();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.CancelMatchmaking();
        }

        public void OnPlayerMatchmade()
        {
            lblWaiting.Content = "Match found! Waiting to enter game...";
        }

        public void OnSelected()
        {
            lblWaiting.Content = "Waiting to be matched...";
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
