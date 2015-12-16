namespace Client.GUI
{
    public interface IGUIGame : IGUIControl
    {
        void OnGameInit();
        void OnTurnTaken();

        void OnShipDestroyed();

        void OnPlayerWon();
        void OnPlayerLost();
    }
}
