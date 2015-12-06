using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShipWarsOnline
{
    class SeaCell
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

        public SeaCellData GetData()
        {
            return new SeaCellData
            {
                ShipIndex = ShipIndex,
                Type = Type,
                Revealed = Revealed
            };
        }
    }

    public class ReadOnlySeaCell
    {
        public 
    }
}
