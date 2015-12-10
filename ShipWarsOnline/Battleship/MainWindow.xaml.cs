using Battleship.GUI;
using System.Windows;
using System;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IGUIWindow
    {
        MainMenuControl mainMenuControl;
        LoginControl loginControl;
        GameControl gameControl;
        LobbyControl lobbyControl;
        AccountCreationControl accountCreationControl;
        MatchmakingControl matchmakingControl;

        public MainWindow()
        {
            InitializeComponent();

            GUIFacade.Instance.WindowContainer = this;
        }

        public void SetDataContext(FrameworkElement element)
        {
            DataContext = element;
        }

        public IGUIControl GetMainMenuControl()
        {
            // Lazy instantiation
            if (mainMenuControl == null)
            {
                mainMenuControl = new MainMenuControl();
            }

            return mainMenuControl;
        }

        public IGUILogin GetLoginControl()
        {
            // Lazy instantiation
            if (loginControl == null)
            {
                loginControl = new LoginControl();
            }

            return loginControl;
        }

        public IGUIControl GetAccountCreationControl()
        {
            // Lazy instantiation
            if (accountCreationControl == null)
            {
                accountCreationControl = new AccountCreationControl();
            }

            return accountCreationControl;
        }

        public IGUILobby GetLobbyControl()
        {
            // Lazy instantiation
            if (lobbyControl == null)
            {
                lobbyControl = new LobbyControl();
            }

            return lobbyControl;
        }

        public IGUIMatchmaking GetMatchmakingControl()
        {
            // Lazy instantiation
            if (matchmakingControl == null)
            {
                matchmakingControl = new MatchmakingControl();
            }

            return matchmakingControl;
        }

        public IGUIGame GetGameControl()
        {
            // Lazy instantiation
            if (gameControl == null)
            {
                gameControl = new GameControl();
            }

            return gameControl;
        }
    }
}
