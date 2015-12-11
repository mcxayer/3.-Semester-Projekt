using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;

namespace ShipWarsOnline
{
    /// <summary>
    /// Game hosted locally taking turns for both players.
    /// </summary>
    public class LocalGame : IGame
    {
        private static readonly int MaxPlayerAmount = 2;

        private IPlayer[] players;

        private SeaGrid[] grids;
        private List<ReadOnlySeaGrid> readOnlyGrids;
        public IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get; private set; }

        public int CurrentPlayerTurn { get; private set; }

        public LocalGame(IPlayer player1, IPlayer player2) : this(player1, player2, SeaGrid.DefaultGridSize) { }
        public LocalGame(IPlayer player1, IPlayer player2, int gridSize)
        {
            players = new IPlayer[MaxPlayerAmount];
            players[0] = player1;
            players[1] = player2;

            grids = new SeaGrid[MaxPlayerAmount];
            readOnlyGrids = new List<ReadOnlySeaGrid>(MaxPlayerAmount);
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i] = new SeaGrid(gridSize);
                readOnlyGrids.Add(new ReadOnlySeaGrid(grids[i]));
            }
            ReadOnlyGrids = readOnlyGrids.AsReadOnly();
        }

        public void AddShip(ShipType type)
        {
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i].AddShip(type);
            }
        }

        public void AddShip(ShipType type, int playerIndex)
        {
            if(playerIndex < 0 || playerIndex >= MaxPlayerAmount)
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            grids[playerIndex].AddShip(type);
        }

        public void AddShip(ShipType type, int playerIndex, int x, int y, bool horizontal)
        {
            if (playerIndex < 0 || playerIndex >= MaxPlayerAmount)
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            grids[playerIndex].AddShip(type,x,y,horizontal);
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
