using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWarsOnline
{
    [Serializable]
    public enum ShipType { Carrier, Battleship, Destroyer, Submarine, PatrolBoat };

    [Serializable]
    public class Ship
    {
        private int health;
        private ShipType type;

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
    }
}
