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
        MainMenuWindow mainMenuWindow;
        LoginWindow loginWindow;
        GameWindow gameWindow;

        
        public MainWindow()
        {
            mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.SetMainWindow(this);
            loginWindow = new LoginWindow();
            loginWindow.SetMainWindow(this);
            gameWindow = new GameWindow();
            gameWindow.SetMainWindow(this);

            InitializeComponent();
            GotoMainMenu();
        }

        private void ExecutedNewGame(object sender, ExecutedRoutedEventArgs e)
        {
            if(DataContext != gameWindow)
            {
                return;
            }

            gameWindow.NewGame();
        }

        private void ExecutedExit(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public void GotoMainMenu()
        {
            DataContext = mainMenuWindow;
        }

        public void GotoLogin()
        {
            DataContext = loginWindow;
        }

        public void GotoGame()
        {
            DataContext = gameWindow;
        }
    }
}
