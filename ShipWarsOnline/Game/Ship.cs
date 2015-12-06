using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWarsOnline
{
    public class Ship
    {
        private int health;
        private ShipType type;

        // Maybe ship factory
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
            this.type = type;
            health = shipLengths[type];
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

            type = data.Type;
            health = data.Health;
        }

        public int Length
        {
            get
            {
                return shipLengths[type];
            }
        }

        public bool Sunk
        {
            get
            {
                return health <= 0;
            }
        }

        public void Damage()
        {
            if(Sunk)
            {
                return;
            }

            health--;
        }

        public ShipData GetData()
        {
            return new ShipData
            {
                Type = type,
                Health = health
            };
        }
    }
}
