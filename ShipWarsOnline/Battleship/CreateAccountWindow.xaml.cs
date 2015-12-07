using System.Windows;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : UserControl
    {
        private MainWindow window;
        public CreateAccountWindow()
        {
            InitializeComponent();
        }
        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void back_Button_Click(object sender, RoutedEventArgs e)
        {
            window.GotoLogin();
        }

        private void createUser_Button_Click(object sender, RoutedEventArgs e)
        {
            GameContextFacade.Instance.CreateAccount(tbUsername.Text,tbPassword.Text,tbEmail.Text);
            window.GotoLogin();
        }

    }
}
