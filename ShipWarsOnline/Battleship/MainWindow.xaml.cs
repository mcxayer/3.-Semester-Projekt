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
            InitializeComponent();
            GotoMainMenu();
        }

        public void GotoMainMenu()
        {
            // Lazy instantiation
            if(mainMenuWindow == null)
            {
                mainMenuWindow = new MainMenuWindow();
                mainMenuWindow.SetMainWindow(this);
            }

            DataContext = mainMenuWindow;
        }

        public void GotoLogin()
        {
            // Lazy instantiation
            if (loginWindow == null)
            {
                loginWindow = new LoginWindow();
                loginWindow.SetMainWindow(this);
            }

            DataContext = loginWindow;
        }

        public void GotoGame()
        {
            // Lazy instantiation
            if (gameWindow == null)
            {
                gameWindow = new GameWindow();
                gameWindow.SetMainWindow(this);
            }

            DataContext = gameWindow;
        }

        public void GotoLobby()
        {
            // Lazy instantiation
            if (lobbyWindow == null)
            {
                lobbyWindow = new LobbyWindow();
                lobbyWindow.SetMainWindow(this);
            }

            lobbyWindow.UpdateLobbyList();
            DataContext = lobbyWindow;
        }

        public void GotoCreateAccount()
        {
            // Lazy instantiation
            if (createAccountWindow == null)
            {
                createAccountWindow = new CreateAccountWindow();
                createAccountWindow.SetMainWindow(this);
            }

            DataContext = createAccountWindow;
        }

        public void GotoMatchmaking()
        {
            // Lazy instantiation
            if (matchmakingWindow == null)
            {
                matchmakingWindow = new MatchmakingWindow();
                matchmakingWindow.SetMainWindow(this);
            }

            DataContext = matchmakingWindow;
        }
    }
}
