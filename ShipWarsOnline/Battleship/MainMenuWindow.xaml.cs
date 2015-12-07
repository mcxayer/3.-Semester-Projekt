using System.Windows;
using System.Windows.Controls;

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
