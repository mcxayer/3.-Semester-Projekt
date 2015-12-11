using ShipWarsOnline.Data;
using System;

namespace ShipWarsOnline
{
    public class SeaCell
    {
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
    }
}
