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
