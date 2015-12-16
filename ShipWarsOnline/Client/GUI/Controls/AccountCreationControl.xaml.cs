using System.Windows;
using System.Windows.Controls;

namespace Client.GUI.Controls
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class AccountCreationControl : UserControl, IGUIAccountCreation
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
            lblInfo.Content = "";
            EnableControl(false);
            GUIFacade.Instance.CreateAccount(tbUsername.Text,tbPassword.Text,tbEmail.Text);
        }

        public void OnSelected()
        {
            tbUsername.Text = "";
            tbPassword.Text = "";
            tbEmail.Text = "";
            lblInfo.Content = "";

            EnableControl(true);
        }

        public void OnPlayerAccountCreated()
        {
            EnableControl(true);
            lblInfo.Content = "Created account!";
        }

        public void OnPlayerAccountFailedCreation()
        {
            EnableControl(true);
            lblInfo.Content = "Failed to create account!";
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
