using Game.Data;

namespace Game.ReadOnly
{
    /// <summary>
    /// Read-only wrapper of Ship to be used outside the game context
    /// </summary>
    public class ReadOnlyShip
    {
        private Ship ship;

        public int Length { get { return ship.Length; } }
        public bool Sunk { get { return ship.Sunk; } }
        public int Health { get { return ship.Health; } }
        public ShipType Type { get { return ship.Type; } }
        public int PosX { get { return ship.PosX; } }
        public int PosY { get { return ship.PosY; } }
        public bool Horizontal { get { return ship.Horizontal; } }

        public ReadOnlyShip(Ship ship)
        {
            this.ship = ship;
        }
    }
}
