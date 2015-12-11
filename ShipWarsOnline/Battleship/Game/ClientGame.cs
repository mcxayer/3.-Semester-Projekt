using GameService;
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

        public ClientGame(GameInitStateDTO initState)
        {
            game = new LocalGame(null,null, initState.GridSize);
            PlayerIndex = initState.PlayerIndex;

            for (int i = 0; i < initState.Ships.Length; i++)
            {
                ShipData ship = initState.Ships[i];
                AddShip(ship.Type, ship.PosX, ship.PosY, ship.Horizontal);
            }
        }

        private void AddShip(ShipType type)
        {
            game.AddShip(type,PlayerIndex);
        }

        private void AddShip(ShipType type, int x, int y, bool horizontal)
        {
            game.AddShip(type, PlayerIndex, x, y, horizontal);
        }

        public void TakeTurn(int x, int y)
        {
            game.TakeTurn(x, y);
        }
    }
}
