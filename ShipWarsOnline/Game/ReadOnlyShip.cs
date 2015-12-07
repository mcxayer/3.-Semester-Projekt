using ShipWarsOnline.Data;

namespace ShipWarsOnline
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

        public ReadOnlyShip(Ship ship)
        {
            this.ship = ship;
        }
    }
}
