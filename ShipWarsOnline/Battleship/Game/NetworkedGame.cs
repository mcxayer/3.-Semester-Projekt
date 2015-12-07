using GameService;
using System;
using System.Collections.Generic;

namespace ShipWarsOnline
{
    public class NetworkedGame : IGame
    {
        private SeaGrid[] grids;
        public IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get; private set; }

        public NetworkedGame(GameStateDTO state)
        {
            // Do stuff
        }

        public void TakeTurn(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
