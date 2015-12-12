using Battleship.Game;
using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface ICellSelectionListener
    {
        void OnCellSelected(object source, ReadOnlySeaCell cell);
    }
}
