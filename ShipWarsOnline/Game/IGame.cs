using System.Collections.Generic;

namespace ShipWarsOnline
{
    public interface IGame
    {
        void TakeTurn(int x, int y);

        IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get; }
    }
}
