using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;

namespace ShipWarsOnline
{
    /// <summary>
    /// Game hosted locally taking turns for both players.
    /// </summary>
    public class LocalGame
    {
        public static readonly int MaxPlayerAmount = 2;

        private SeaGrid[] grids;
        private List<ReadOnlySeaGrid> readOnlyGrids;
        public IReadOnlyList<ReadOnlySeaGrid> ReadOnlyGrids { get; private set; }

        public int CurrentPlayerTurn { get; private set; }

        public LocalGame() : this(SeaGrid.DefaultGridSize) { }
        public LocalGame(int gridSize)
        {
            Random rnd = new Random();

            grids = new SeaGrid[MaxPlayerAmount];
            readOnlyGrids = new List<ReadOnlySeaGrid>(MaxPlayerAmount);
            for (int i = 0; i < grids.Length; i++)
            {
                grids[i] = new SeaGrid(gridSize, rnd);
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
            if(!IsPlayerValid(playerIndex))
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            grids[playerIndex].AddShip(type);
        }

        public void AddShip(ShipType type, int playerIndex, int x, int y, bool horizontal)
        {
            if (!IsPlayerValid(playerIndex))
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
        }

        public void ShowPlayerGrid(int playerIndex)
        {
            if (!IsPlayerValid(playerIndex))
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            grids[playerIndex].ShowAll();
        }

        public void SetCellType(int playerIndex, int x, int y, CellType type)
        {
            if (!IsPlayerValid(playerIndex))
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            grids[playerIndex].SetCellType(x, y, type);
        }

        public Bounds GetDestroyedShip(int playerIndex)
        {
            if (!IsPlayerValid(playerIndex))
            {
                throw new ArgumentOutOfRangeException("playerIndex");
            }

            return grids[playerIndex].DestroyedShipBounds;
        }

        private bool IsPlayerValid(int playerIndex)
        {
            return playerIndex >= 0 && playerIndex < MaxPlayerAmount;
        }

        public int GetWinner()
        {
            int winnerIndex = -1;
            if(grids[0].AreAllShipsSunk())
            {
                winnerIndex = 1;
            }
            else if(grids[1].AreAllShipsSunk())
            {
                winnerIndex = 0;
            }

            return winnerIndex;
        }

        public bool IsGameOver()
        {
            return grids[0].AreAllShipsSunk() || grids[1].AreAllShipsSunk();
        }
    }
}
