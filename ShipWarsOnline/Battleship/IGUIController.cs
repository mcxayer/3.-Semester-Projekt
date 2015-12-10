using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public interface IGUIController
    {
        void GotoWindow(GUIWindowType type);
    }
}
