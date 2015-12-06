using Battleship.Game;
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
using Grid = Battleship.Game.Grid;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        private MainWindow window;

        private Player _player1;
        private Player _player2;

        private Grid _mygrid;
        private Grid _enemygrid;

        public GameWindow()
        {
            _player1 = new Player();
            _player2 = new Player();
            _mygrid = new Grid(_player1, _player2);
            _enemygrid = new Grid(_player1, _player2);

            InitializeComponent();

            myGrid.DataContext = _mygrid;
            enemyGrid.DataContext = _enemygrid;
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        public void NewGame()
        {
            _player1.Reset();
            _player2.Reset();
        }
    }
}
