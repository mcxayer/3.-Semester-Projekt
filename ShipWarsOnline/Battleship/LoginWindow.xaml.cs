using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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
            window.GotoGame();
        }

        private void OnLoginButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                string tokenId = ServiceFacade.Instance.Login(tbUsername.Text, tbPassword.Text);
                if (string.IsNullOrEmpty(tokenId))
                {
                    lblInfo.Content = "Unknown error occurred";
                    return;
                }
            }
            catch(FaultException ex)
            {
                lblInfo.Content = ex.Message;
                return;
            }

            window.GotoGame();
        }
    }
}
