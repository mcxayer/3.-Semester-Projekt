using Battleship.Game;
using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : UserControl
    {
        public CreateAccountWindow()
        {
            InitializeComponent();
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoLogin();
        }

        private void OnCreateUserButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.CreateAccount(tbUsername.Text,tbPassword.Text,tbEmail.Text);
        }
    }
}
