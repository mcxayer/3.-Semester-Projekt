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

        private string[] playerNames;
        public IReadOnlyList<string> ReadOnlyPlayerNames { get; private set; }

        public ClientGame(GameInitStateDTO initState)
        {
            game = new LocalGame(initState.GridSize);
            PlayerIndex = initState.PlayerIndex;

            playerNames = new string[LocalGame.MaxPlayerAmount];
            playerNames[PlayerIndex] = initState.PlayerName;
            playerNames[(PlayerIndex + 1) % LocalGame.MaxPlayerAmount] = initState.OpponentName;
            ReadOnlyPlayerNames = new List<string>(playerNames).AsReadOnly();

            for (int i = 0; i < initState.Ships.Length; i++)
            {
                ShipData ship = initState.Ships[i];
                AddShip(ship.Type, ship.PosX, ship.PosY, ship.Horizontal);
            }

            game.ShowPlayerGrid(PlayerIndex);
        }

        private void AddShip(ShipType type)
        {
            game.AddShip(type,PlayerIndex);
        }

        private void AddShip(ShipType type, int x, int y, bool horizontal)
        {
            game.AddShip(type, PlayerIndex, x, y, horizontal);
        }

        public void AddDestroyedShip(Bounds shipBounds)
        {
            if(shipBounds == null)
            {
                throw new ArgumentNullException("shipBounds");
            }

            for (int i = shipBounds.MinX; i <= shipBounds.MaxX; i++)
            {
                for (int j = shipBounds.MinY; j <= shipBounds.MaxY; j++)
                {
                    game.SetCellType((PlayerIndex + 1) % 2, i, j, CellType.Sunk);
                }
            }
        }

        public void TakeTurn(int x, int y)
        {
            game.TakeTurn(x, y);
        }

        public void TakeTurn(int x, int y, CellType type)
        {
            game.TakeTurn(x, y);
            game.SetCellType(game.CurrentPlayerTurn, x, y, type);
        }

        public string GetPlayerName()
        {
            return playerNames[PlayerIndex];
        }

        public string GetOpponentName()
        {
            return playerNames[(PlayerIndex + 1) % LocalGame.MaxPlayerAmount];
        }

        public bool IsPlayerTurn()
        {
            return PlayerIndex == game.CurrentPlayerTurn;
        }
    }
}
