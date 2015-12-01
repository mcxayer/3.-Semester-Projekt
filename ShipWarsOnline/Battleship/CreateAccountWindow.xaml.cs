using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            ServiceFacade.Instance.CreateAccount(tbUsername.Text,tbPassword.Text,tbEmail.Text);
            window.GotoLogin();
        }

    }
}
