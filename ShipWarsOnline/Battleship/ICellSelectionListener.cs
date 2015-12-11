using Battleship.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface ICellSelectionListener
    {
        void OnCellSelected(SeaSquare cell);
    }
}
