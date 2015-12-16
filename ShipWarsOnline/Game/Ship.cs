using GameData;
using System;
using System.Collections.Generic;

namespace Game
{
    public class Ship
    {
        public int Length { get { return shipLengths[Type]; } }
        public bool Sunk { get { return Health <= 0; } }

        public int Health { get; private set; }
        public ShipType Type { get; private set; }

        public int PosX { get; private set; }
        public int PosY { get; private set; }
        public bool Horizontal { get; private set; }

        private static readonly Dictionary<ShipType, int> shipLengths = new Dictionary<ShipType, int>()
        {
            {ShipType.Carrier, 5},
            {ShipType.Battleship, 4},
            {ShipType.Destroyer, 3},
            {ShipType.Submarine, 3},
            {ShipType.PatrolBoat, 2}
        };

        public Ship(ShipType type, int posX, int posY, bool horizontal)
        {
            Type = type;
            Health = GetShipLength(type);
            PosX = posX;
            PosY = posY;
            Horizontal = horizontal;
        }

        public void Damage()
        {
            if(Sunk)
            {
                return;
            }

            Health--;
        }

        public static int GetShipLength(ShipType type)
        {
            switch(type)
            {
                case ShipType.Carrier: return 5;
                case ShipType.Battleship: return 4;
                case ShipType.Destroyer: return 3;
                case ShipType.Submarine: return 3;
                case ShipType.PatrolBoat: return 2;
                default: throw new Exception("Failed to match type with length!");
            }
        }
    }
}
