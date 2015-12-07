using System.Collections.Generic;

namespace ShipWarsOnline
{
    public class LocalGame : IGame
    {
        private static readonly int MaxPlayerAmount = 2;

        private IPlayer[] players;

        private SeaGrid[] grids;
        public IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get; private set; }

        public int CurrentPlayerTurn { get; private set; }

        public LocalGame(IPlayer player1, IPlayer player2)
        {
            players = new IPlayer[MaxPlayerAmount];
            players[0] = player1;
            players[1] = player2;

            grids = new SeaGrid[MaxPlayerAmount];
            List<ReadOnlySeaGrid> roGrids = new List<ReadOnlySeaGrid>(MaxPlayerAmount);
            for (int i = 0; i < MaxPlayerAmount; i++)
            {
                grids[i] = new SeaGrid();
                roGrids.Add(new ReadOnlySeaGrid(grids[i]));
            }
            ReadOnlyGrids = roGrids.AsReadOnly();
        }

        public void TakeTurn(int x, int y)
        {
            int nextPlayerIndex = (CurrentPlayerTurn + 1) % MaxPlayerAmount;
            grids[nextPlayerIndex].FireAt(x,y);

            CurrentPlayerTurn = nextPlayerIndex;

            // Check winner here
        }
    }
}
