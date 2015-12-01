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
        LobbyWindow lobbyWindow;
        CreateAccountWindow createAccountWindow;
        MatchmakingWindow matchmakingWindow;

        
        public MainWindow()
        {
            mainMenuWindow = new MainMenuWindow();
            mainMenuWindow.SetMainWindow(this);
            loginWindow = new LoginWindow();
            loginWindow.SetMainWindow(this);
            gameWindow = new GameWindow();
            gameWindow.SetMainWindow(this);
            lobbyWindow = new LobbyWindow();
            lobbyWindow.SetMainWindow(this);
            createAccountWindow = new CreateAccountWindow();
            createAccountWindow.SetMainWindow(this);
            matchmakingWindow = new MatchmakingWindow();
            matchmakingWindow.SetMainWindow(this);

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

        public void GotoLobby()
        {
            lobbyWindow.updateLobbyList();
            DataContext = lobbyWindow;
        }

        public void GotoCreateAccount()
        {
            DataContext = createAccountWindow;
        }

        public void GotoMatchmaking()
        {
            DataContext = matchmakingWindow;
        }
    }
}
