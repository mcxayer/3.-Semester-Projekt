using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;

namespace ShipWarsOnline
{
    public class Ship
    {
        public int Length { get { return shipLengths[Type]; } }
        public bool Sunk { get { return Health <= 0; } }

        public int Health { get; private set; }
        public ShipType Type { get; private set; }

        // Maybe ship factory and immutable object
        private static readonly Dictionary<ShipType, int> shipLengths = new Dictionary<ShipType, int>()
        {
            {ShipType.Carrier, 5},
            {ShipType.Battleship, 4},
            {ShipType.Destroyer, 3},
            {ShipType.Submarine, 3},
            {ShipType.PatrolBoat, 2}
        };

        public Ship(ShipType type)
        {
            Type = type;
            Health = shipLengths[type];
        }

        public Ship(ShipData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Health < 0 || data.Health > shipLengths[data.Type])
            {
                throw new Exception("Health of ship must not be less than zero or greater than maximum health!");
            }

            Type = data.Type;
            Health = data.Health;
        }

        public void Damage()
        {
            if(Sunk)
            {
                return;
            }

            Health--;
        }
    }
}
