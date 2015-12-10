using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuWindow : UserControl
    {
        public MainMenuWindow()
        {
            InitializeComponent();
        }

        private void OnStartMultiplayerButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoLogin();
        }
    }
}
