using System.Collections.Generic;

namespace Client.GUI
{
    public interface IGUILobby : IGUIControl
    {
        void OnLobbyUpdated(List<string> lobbyNames);
    }
}
