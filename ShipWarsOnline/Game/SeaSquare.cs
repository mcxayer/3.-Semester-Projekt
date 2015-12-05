using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShipWarsOnline
{
    [Serializable]
    public enum SquareType { Unknown, Water, Undamaged, Damaged, Sunk }

    [Serializable]
    public class SeaSquare
    {
        private int shipIndex;
        public int ShipIndex
        {
            get { return shipIndex; }
            set
            {
                if(shipIndex < -1)
                {
                    throw new ArgumentOutOfRangeException("ShipIndex.value");
                }

                shipIndex = value;
            }
        }

        public SquareType Type { get; set; }
    }
}
