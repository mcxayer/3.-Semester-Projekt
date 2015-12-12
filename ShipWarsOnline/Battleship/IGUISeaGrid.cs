using Battleship.Game;
using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IGUISeaGrid
    {
        ReadOnly2DArray<ReadOnlySeaCell> GridCells { get; set; }
        ICellSelectionListener CellSelectionCallback { get; set; }
        void OnTurnTaken();
    }
}
