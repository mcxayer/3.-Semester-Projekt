using ShipWarsOnline.Data;
using System;

namespace ShipWarsOnline
{
    public class SeaCell
    {
        private int shipIndex;
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

        public SeaCell() { }

        public SeaCell(SeaCellData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.ShipIndex < -1)
            {
                throw new Exception("Ship index must not be less than negative one!");
            }

            ShipIndex = data.ShipIndex;
            Type = data.Type;
            Revealed = data.Revealed;
        }
    }
}
