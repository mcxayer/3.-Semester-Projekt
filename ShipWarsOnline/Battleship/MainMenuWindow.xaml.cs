using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuWindow : UserControl
    {
        private MainWindow window;

        public MainMenuWindow()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void btnMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            window.GotoLogin();
        }
    }
}
