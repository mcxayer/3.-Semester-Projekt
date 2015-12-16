namespace Client.GUI
{
    public interface IGUILogin : IGUIControl
    {
        void OnPlayerConnected();
        void OnPlayerFailedConnecting();
    }
}
