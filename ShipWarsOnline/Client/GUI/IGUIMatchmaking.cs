namespace Client.GUI
{
    public interface IGUIMatchmaking : IGUIControl
    {
        void OnPlayerMatchmade();
        void OnPlayerCancelledMatchmaking();
    }
}
