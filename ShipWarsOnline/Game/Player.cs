using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWarsOnline
{
    public class Player : IPlayer
    {
        public SeaGrid Grid { get; set; }

        public void TakeTurn(int x, int y)
        {
            Grid.FireAt(x, y);
        }
    }
}
