using GameData;
using System;

namespace Game
{
    public class SeaCell
    {
        public int PosX { get; private set; }
        public int PosY { get; private set; }

        private int shipIndex = -1;
        public int ShipIndex
        {
            get { return shipIndex; }
            set
            {
                if(value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                shipIndex = value;
            }
        }

        public CellType Type { get; set; }

        public bool Revealed { get; set; }

        public SeaCell(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
    }
}
