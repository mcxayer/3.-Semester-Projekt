using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;

namespace Battleship.Game
{
    /// <summary>
    /// Client game mimicking a hosted local game on a server. It only takes turns for one player.
    /// </summary>
    public class ClientGame
    {
        private LocalGame game;

        public int PlayerIndex { get; private set; }
        public IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get { return game.ReadOnlyGrids; } }

        public ClientGame(int gridSize, int playerIndex)
        {
            game = new LocalGame(null,null, gridSize);
            PlayerIndex = playerIndex;
        }

        public void AddShip(ShipType type)
        {
            game.AddShip(type,PlayerIndex);
        }

        public void TakeTurn(int x, int y)
        {
            game.TakeTurn(x, y);
        }
    }
}
