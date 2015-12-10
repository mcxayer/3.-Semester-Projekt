using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;
using System;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl, IGUIControl
    {
        public MainMenuControl()
        {
            InitializeComponent();
        }

        private void OnStartMultiplayerButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoLogin();
        }

        public void OnSelected()
        {
            // Nothing
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
