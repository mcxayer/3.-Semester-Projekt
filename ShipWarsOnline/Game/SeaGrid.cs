using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWarsOnline
{
    public class SeaGrid
    {
        public static readonly int DefaultGridSize = 10;
        private static readonly int MaxLoopCount = 10000;

        private Random rnd = new Random();

        public int Size { get; private set; }

        private SeaCell[,] cells;
        public ReadOnly2DArray<ReadOnlySeaCell> ReadOnlyCells { get; private set; }

        private List<Ship> ships;
        private List<ReadOnlyShip> readOnlyShips;
        public IReadOnlyList<ReadOnlyShip> ReadOnlyShips { get; private set; }

        public SeaGrid() : this(DefaultGridSize) { }
        public SeaGrid(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "size must be greater than zero!");
            }
            Size = size;

            cells = new SeaCell[Size, Size];
            ReadOnlySeaCell[,] roCells = new ReadOnlySeaCell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    cells[i, j] = new SeaCell();
                    roCells[i, j] = new ReadOnlySeaCell(cells[i, j]);
                }
            }
            ReadOnlyCells = new ReadOnly2DArray<ReadOnlySeaCell>(roCells);

            ships = new List<Ship>();
            readOnlyShips = new List<ReadOnlyShip>();
            ReadOnlyShips = readOnlyShips.AsReadOnly();

            Reset();
        }

        #region private methods

        private void PlaceShips()
        {
            for (int i = 0; i < ships.Count; ++i)
            {
                if (!PlaceShip(i, ships[i].Length))
                {
                    throw new Exception(string.Format("Unable to place ship {0} with length {1}!", i, ships[i].Length));
                }
            }
        }

        private bool PlaceShip(int shipIndex, int length)
        {
            bool horizontal = false;
            bool validPlacement = false;
            int startX = 0, startY = 0, endX = 0, endY = 0;

            for (int i = 0; i < MaxLoopCount; i++)
            {
                horizontal = rnd.Next(2) == 0;

                startX = rnd.Next(horizontal ? Size - length : Size);
                startY = rnd.Next(horizontal ? Size : Size - length);

                endX = horizontal ? startX + length : startX;
                endY = horizontal ? startY : startY + length;

                for (int j = 0; j < Size - endX; j++)
                {
                    if (IsPlacementValid(startX + j, startY, endX + j, endY))
                    {
                        startX += j;
                        endX += j;

                        validPlacement = true;

                        break;
                    }
                }

                if(validPlacement)
                {
                    break;
                }

                if(i == MaxLoopCount - 1)
                {
                    return false;
                }
            }

            for (int j = 0; j < length; j++)
            {
                SeaCell square = horizontal
                    ? GetCellInternal(startX + j, startY)
                    : GetCellInternal(startX, startY + j);

                square.Type = CellType.Undamaged;
                square.ShipIndex = shipIndex;
            }

            return true;
        }

        private bool IsPlacementValid(int startX, int startY, int endX, int endY)
        {
            return (endX - startX > 0)
                ? IsHorizontalPlacementValid(startY, startX, endX)
                : IsVerticalPlacementValid(startX, startY, endY);
        }

        private bool IsVerticalPlacementValid(int x, int startY, int endY)
        {
            for (int i = startY; i <= endY; i++)
            {
                if (!IsSquareValid(x, i) || !IsSquareFree(x, i))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsHorizontalPlacementValid(int y, int startX, int endX)
        {
            for (int i = startX; i <= endX; i++)
            {
                if (!IsSquareValid(i, y) || !IsSquareFree(i, y))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSquareFree(int x, int y)
        {
            return GetCellInternal(x, y).ShipIndex == -1;
        }

        private bool IsSquareValid(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        private Ship GetShipInternal(int index)
        {
            if (index < 0 || index >= ships.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return ships[index];
        }

        private void SinkShip(int shipIndex)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    SeaCell square = GetCellInternal(i, j);
                    if (square.ShipIndex == shipIndex)
                    {
                        square.Type = CellType.Sunk;
                    }
                }
            }
        }

        private SeaCell GetCellInternal(int x, int y)
        {
            if (x < 0 || x >= Size)
            {
                throw new ArgumentOutOfRangeException("x");
            }

            if (y < 0 || y >= Size)
            {
                throw new ArgumentOutOfRangeException("y");
            }

            return cells[x, y];
        }

        #endregion

        #region public methods

        public void AddShip(ShipType type)
        {
            Ship ship = new Ship(type);

            if (!PlaceShip(ships.Count, ship.Length))
            {
                throw new Exception(string.Format("Unable to place ship {0} with length {1}!", ships.Count, ship.Length));
            }

            ships.Add(ship);
        }

        public void Reset()
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    SeaCell square = GetCellInternal(i, j);
                    square.Type = CellType.Water;
                    square.ShipIndex = -1;
                }
            }

            PlaceShips();
        }

        public CellType FireAt(int x, int y)
        {
            SeaCell square = GetCellInternal(x, y);

            if (square.Type == CellType.Undamaged)
            {
                int indexToDamage = square.ShipIndex;
                Ship ship = GetShipInternal(indexToDamage);
                ship.Damage();

                if (ship.Sunk)
                {
                    SinkShip(indexToDamage);
                }
                else
                {
                    square.Type = CellType.Damaged;
                }
            }

            return square.Type;
        }

        public bool AreAllShipsSunk()
        {
            return ships.All(ship => ship.Sunk);
        }

        #endregion
    }
}
