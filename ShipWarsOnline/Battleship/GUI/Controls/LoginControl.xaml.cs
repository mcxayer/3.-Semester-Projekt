using Battleship.Game;
using Battleship.GUI;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginControl : UserControl, IGUILogin
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoMainMenu();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            EnableControl(false);
            GUIFacade.Instance.LoginAndConnectLobby(tbUsername.Text, tbPassword.Password);
        }

        private void OnCreateUserButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoAccountCreation();
        }

        public void OnPlayerConnected()
        {
            EnableControl(true);
        }

        public void OnPlayerFailedConnecting()
        {
            lblInfo.Content = "Failed to connect!";
            EnableControl(true);
        }

        public void OnSelected()
        {
            lblInfo.Content = "";
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
