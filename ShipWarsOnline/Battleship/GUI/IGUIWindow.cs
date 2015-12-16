using System.Windows;

namespace Client.GUI
{
    public enum GUIWindowType
    {
        MainMenu,
        Login,
        AccountCreation,
        Lobby,
        Matchmaking,
        Game
    }

    public interface IGUIWindow
    {
        void SetDataContext(FrameworkElement element);
        IGUIControl GetMainMenuControl();
        IGUILogin GetLoginControl();
        IGUIAccountCreation GetAccountCreationControl();
        IGUILobby GetLobbyControl();
        IGUIMatchmaking GetMatchmakingControl();
        IGUIGame GetGameControl();
    }
}
