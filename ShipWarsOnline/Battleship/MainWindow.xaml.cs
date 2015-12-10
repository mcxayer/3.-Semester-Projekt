using Battleship.GUI;
using System.Windows;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IGUIController
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

            GUIFacade.Instance.GUIController = this;
        }

        public void GotoWindow(GUIWindowType type)
        {
            switch(type)
            {
                case GUIWindowType.MainMenu: GotoMainMenu();
                    break;
                case GUIWindowType.Login: GotoLogin();
                    break;
                case GUIWindowType.AccountCreation: GotoAccountCreation();
                    break;
                case GUIWindowType.Lobby: GotoLobby();
                    break;
                case GUIWindowType.Matchmaking: GotoMatchmaking();
                    break;
                case GUIWindowType.Game: GotoGame();
                    break;

                default:
                    throw new System.Exception(string.Format("Window type {0} is not valid!",type));
            }
        }

        private void GotoMainMenu()
        {
            // Lazy instantiation
            if(mainMenuWindow == null)
            {
                mainMenuWindow = new MainMenuWindow();
            }

            DataContext = mainMenuWindow;
        }

        private void GotoLogin()
        {
            // Lazy instantiation
            if (loginWindow == null)
            {
                loginWindow = new LoginWindow();
            }

            DataContext = loginWindow;
        }

        private void GotoAccountCreation()
        {
            // Lazy instantiation
            if (createAccountWindow == null)
            {
                createAccountWindow = new CreateAccountWindow();
            }

            DataContext = createAccountWindow;
        }

        private void GotoLobby()
        {
            // Lazy instantiation
            if (lobbyWindow == null)
            {
                lobbyWindow = new LobbyWindow();
            }

            lobbyWindow.UpdateLobbyList();
            DataContext = lobbyWindow;
        }

        private void GotoMatchmaking()
        {
            // Lazy instantiation
            if (matchmakingWindow == null)
            {
                matchmakingWindow = new MatchmakingWindow();
            }

            DataContext = matchmakingWindow;
        }

        private void GotoGame()
        {
            // Lazy instantiation
            if (gameWindow == null)
            {
                gameWindow = new GameWindow();
            }

            DataContext = gameWindow;
        }
    }
}
