using Game;
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

        private Random rnd;

        public int Size { get; private set; }

        private SeaCell[,] cells;
        public ReadOnly2DArray<ReadOnlySeaCell> ReadOnlyCells { get; private set; }

        private List<Ship> ships;
        private List<ReadOnlyShip> readOnlyShips;
        public IReadOnlyList<ReadOnlyShip> ReadOnlyShips { get; private set; }

        public Bounds DestroyedShip { get; private set; }

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
                    cells[i, j] = new SeaCell(i, j);
                    roCells[i, j] = new ReadOnlySeaCell(cells[i, j]);
                }
            }
            ReadOnlyCells = new ReadOnly2DArray<ReadOnlySeaCell>(roCells);

            ships = new List<Ship>();
            readOnlyShips = new List<ReadOnlyShip>();
            ReadOnlyShips = readOnlyShips.AsReadOnly();

            rnd = new Random();

            Reset();
        }

        #region private methods

        private void PlaceShips()
        {
            for (int i = 0; i < ships.Count; ++i)
            {
                if (!TryPlaceShip(ships[i]))
                {
                    throw new Exception(string.Format("Unable to place ship {0} with length {1}!", i, ships[i].Length));
                }
            }
        }

        private bool TryPlaceShip(Ship ship)
        {
            bool horizontal = false;
            bool validPlacement = false;
            int startX = 0, startY = 0, endX = 0, endY = 0;

            for (int i = 0; i < MaxLoopCount; i++)
            {
                horizontal = rnd.Next(2) == 0;

                startX = rnd.Next(horizontal ? Size - ship.Length : Size);
                startY = rnd.Next(horizontal ? Size : Size - ship.Length);

                endX = horizontal ? startX + (ship.Length - 1) : startX;
                endY = horizontal ? startY : startY - (ship.Length - 1);

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

                if (validPlacement)
                {
                    break;
                }

                if (i == MaxLoopCount - 1)
                {
                    return false;
                }
            }

            PlaceShip(ship, startX, startY, horizontal);

            return true;
        }

        private bool TryPlaceShip(Ship ship, int startX, int startY, bool horizontal)
        {
            int endX = horizontal ? startX + (ship.Length - 1) : startX;
            int endY = horizontal ? startY : startY - (ship.Length - 1);

            if (!IsPlacementValid(startX, startY, endX, endY))
            {
                return false;
            }

            PlaceShip(ship, startX, startY, horizontal);

            return true;
        }

        private void PlaceShip(Ship ship, int startX, int startY, bool horizontal)
        {
            ship.PosX = startX;
            ship.PosY = startY;
            ship.Horizontal = horizontal;

            int shipIndex = ships.Count;
            ships.Add(ship);
            readOnlyShips.Add(new ReadOnlyShip(ship));

            for (int j = 0; j < ship.Length; j++)
            {
                SeaCell square = horizontal
                    ? GetCellInternal(startX + j, startY)
                    : GetCellInternal(startX, startY - j);

                square.Type = CellType.Undamaged;
                square.ShipIndex = shipIndex;
            }
        }

        private bool IsPlacementValid(int startX, int startY, int endX, int endY)
        {
            return (endY - startY == 0)
                ? IsHorizontalPlacementValid(startY, startX, endX)
                : IsVerticalPlacementValid(startX, startY, endY);
        }

        private bool IsVerticalPlacementValid(int x, int startY, int endY)
        {
            for (int i = endY; i <= startY; i++)
            {
                if (!IsCellValid(x, i) || !IsCellFree(x, i))
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
                if (!IsCellValid(i, y) || !IsCellFree(i, y))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsCellFree(int x, int y)
        {
            return GetCellInternal(x, y).ShipIndex == -1;
        }

        private bool IsCellValid(int x, int y)
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
            int minX = 0, minY = 0, maxX = 0, maxY = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    SeaCell cell = GetCellInternal(i, j);
                    if (cell.ShipIndex == shipIndex)
                    {
                        if(i < minX)
                        {
                            minX = i;
                        }
                        else if(i > maxX)
                        {
                            maxX = i;
                        }

                        if (j < minY)
                        {
                            minY = j;
                        }
                        else if (j > maxY)
                        {
                            maxY = j;
                        }

                        cell.Type = CellType.Sunk;
                    }
                }
            }

            if(minX != maxX || minY != maxY)
            {
                DestroyedShip = new Bounds(minX, minY, maxX, maxY);
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
            rnd = new Random();

            Ship ship = new Ship(type);

            if (!TryPlaceShip(ship))
            {
                throw new Exception(string.Format("Unable to place ship {0} with length {1}!", ships.Count, ship.Length));
            }
        }

        public void AddShip(ShipType type, int x, int y, bool horizontal)
        {
            rnd = new Random();

            Ship ship = new Ship(type);

            if (!TryPlaceShip(new Ship(type), x, y, horizontal))
            {
                throw new Exception(string.Format("Unable to place ship {0} with length {1}!", ships.Count, ship.Length));
            }
        }

        public void SetCellType(int x, int y, CellType type)
        {
            SeaCell cell = GetCellInternal(x, y);
            cell.Type = type;
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

        public void FireAt(int x, int y)
        {
            DestroyedShip = null;

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

            square.Revealed = true;
        }

        public bool AreAllShipsSunk()
        {
            return ships.All(ship => ship.Sunk);
        }

        public void ShowAll()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j].Revealed = true;
                }
            }
        }

        #endregion
    }
}
