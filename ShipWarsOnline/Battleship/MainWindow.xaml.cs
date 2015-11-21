using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Battleship.Game;
using Grid = Battleship.Game.Grid;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player _player1;
        Player _player2;

        Grid _mygrid;
        Grid _enermygrid;
        
        public MainWindow()
        {
            _player1 = new Player();
            _player2 = new Player();
            _mygrid = new Grid(_player1, _player2);
            _enermygrid = new Grid(_player1, _player2);

            InitializeComponent();
            myGrid.DataContext = _mygrid;
            enermyGrid.DataContext = _enermygrid;
        }

        private void ExecutedNewGame(object sender, ExecutedRoutedEventArgs e)
        {
            _player1.Reset();
            _player2.Reset();            
        }

        private void ExecutedExit(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
