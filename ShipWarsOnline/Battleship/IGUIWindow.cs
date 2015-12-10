using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Battleship
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
        IGUIControl GetAccountCreationControl();
        IGUILobby GetLobbyControl();
        IGUIMatchmaking GetMatchmakingControl();
        IGUIGame GetGameControl();
    }
}
