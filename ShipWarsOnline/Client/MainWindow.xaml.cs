using Client.GUI.Controls;
using System;
using System.Windows;

namespace Client.GUI
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

            try
            {
                GUIFacade.Instance.WindowContainer = this;
            }
            catch(Exception ex)
            {
                DialogMessageBox mb = new DialogMessageBox();
                mb.Title = "Error!";
                mb.Information = ex.Message;
                mb.ShowDialog();
                Close();
            }
        }

        public void SetDataContext(FrameworkElement element)
        {
            DataContext = element;
        }

        public IGUIControl GetMainMenuControl()
        {
            if (mainMenuControl == null)
            {
                mainMenuControl = new MainMenuControl();
            }

            return mainMenuControl;
        }

        public IGUILogin GetLoginControl()
        {
            if (loginControl == null)
            {
                loginControl = new LoginControl();
            }

            return loginControl;
        }

        public IGUIAccountCreation GetAccountCreationControl()
        {
            if (accountCreationControl == null)
            {
                accountCreationControl = new AccountCreationControl();
            }

            return accountCreationControl;
        }

        public IGUILobby GetLobbyControl()
        {
            if (lobbyControl == null)
            {
                lobbyControl = new LobbyControl();
            }

            return lobbyControl;
        }

        public IGUIMatchmaking GetMatchmakingControl()
        {
            if (matchmakingControl == null)
            {
                matchmakingControl = new MatchmakingControl();
            }

            return matchmakingControl;
        }

        public IGUIGame GetGameControl()
        {
            if (gameControl == null)
            {
                gameControl = new GameControl();
            }

            return gameControl;
        }
    }
}
