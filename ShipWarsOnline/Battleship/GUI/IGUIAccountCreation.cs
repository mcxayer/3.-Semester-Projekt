namespace Client.GUI
{
    public interface IGUIAccountCreation : IGUIControl
    {
        void OnPlayerAccountCreated();
        void OnPlayerAccountFailedCreation();
    }
}
