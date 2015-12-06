using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public interface IGame
    {
        void TakeTurn(int x, int y);

        CellType[][] GetCellTypes(int playerIndex);
    }
}
