using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        private MainWindow window;

        public GameWindow()
        {
            InitializeComponent();
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }
    }
}
