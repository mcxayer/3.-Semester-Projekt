using System.Collections.Generic;

namespace Game.ReadOnly
{
    /// <summary>
    /// Read-only wrapper of SeaGrid to be used outside the game context
    /// </summary>
    public class ReadOnlySeaGrid
    {
        private SeaGrid grid;

        public int Size { get { return grid.Size; } }
        public ReadOnly2DArray<ReadOnlySeaCell> ReadOnlyCells { get { return grid.ReadOnlyCells; } }
        public IReadOnlyList<ReadOnlyShip> ReadOnlyShips { get { return grid.ReadOnlyShips; } }

        public ReadOnlySeaGrid(SeaGrid grid)
        {
            this.grid = grid;
        }
    }
}
