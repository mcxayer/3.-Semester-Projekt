using Battleship.Game;
using Battleship.GUI;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class AccountCreationControl : UserControl, IGUIControl
    {
        public AccountCreationControl()
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

        public void OnSelected()
        {
            tbUsername.Text = "";
            tbPassword.Text = "";
            tbEmail.Text = "";
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
