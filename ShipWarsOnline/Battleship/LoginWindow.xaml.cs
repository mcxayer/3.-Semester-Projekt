using Battleship.Game;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginWindow : UserControl
    {
        private MainWindow window;

        public LoginWindow()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            window.GotoMainMenu();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string tokenId = GameContextFacade.Instance.Login(tbUsername.Text, tbPassword.Password);

                if (string.IsNullOrEmpty(tokenId))
                {
                    lblInfo.Content = "Unknown error occurred";
                    return;
                }
                else
                {
                    GameContextFacade.Instance.Connect(tokenId);
                }
            }
            catch(FaultException ex)
            {
                lblInfo.Content = ex.Message;
                return;
            }


            window.GotoLobby();
        }

        private void createUser_Click(object sender, RoutedEventArgs e)
        {
            window.GotoCreateAccount();
        }
    }
}
