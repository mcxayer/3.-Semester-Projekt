using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipWarsOnline
{
    public class SeaGrid
    {
        private static readonly int MaxLoopCount = 10000;
        private static readonly int DefaultGridSize = 10;

        private Random rnd = new Random();

        public int Size { get; private set; }

        private SeaCell[,] cells;
        public ReadOnly2DArray<ReadOnlySeaCell> ReadOnlyCells { get; private set; }

        private List<Ship> ships;
        public IReadOnlyList<ReadOnlyShip> ReadOnlyShips { get; private set; }

        public SeaGrid() : this(DefaultGridSize) { }
        public SeaGrid(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "size must be greater than zero!");
            }
            this.Size = size;

            cells = new SeaCell[this.Size, this.Size];
            ReadOnlySeaCell[,] roCells = new ReadOnlySeaCell[this.Size, this.Size];
            for (int i = 0; i < this.Size; i++)
            {
                for (int j = 0; j < this.Size; j++)
                {
                    cells[i, j] = new SeaCell();
                    roCells[i, j] = new ReadOnlySeaCell(cells[i, j]);
                }
            }
            ReadOnlyCells = new ReadOnly2DArray<ReadOnlySeaCell>(roCells);

            ships = new List<Ship>();
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                ships.Add(new Ship(type));
            }

            List<ReadOnlyShip> roShips = new List<ReadOnlyShip>();
            foreach(Ship ship in ships)
            {
                roShips.Add(new ReadOnlyShip(ship));
            }
            ReadOnlyShips = roShips.AsReadOnly();

            Reset();
        }

        public SeaGrid(SeaGridData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (data.Ships == null)
            {
                throw new NullReferenceException("Ships in data can not be null!");
            }

            if (data.Cells == null)
            {
                throw new NullReferenceException("Cells in data can not be null!");
            }

            Size = data.Cells.GetLength(0);
            cells = new SeaCell[Size, Size];
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i,j] = new SeaCell(data.Cells[i][j]);
                }
            }

            ships = new List<Ship>();
            for (int i = 0; i < data.Ships.Length; i++)
            {
                ships.Add(new Ship(data.Ships[i]));
            }
        }

        #region private methods

        private void PlaceShips()
        {
            for (int i = 0; i != ships.Count; ++i)
            {
                bool placed = false;

                int loopCounter = 0;
                while (!placed && loopCounter < MaxLoopCount)
                {
                    placed = PlaceShip(i, ships[i].Length);

                    loopCounter++;
                }

                if (loopCounter == MaxLoopCount)
                {
                    throw new Exception(string.Format("Unable to place ship {0}!", i));
                }
            }
        }

        private bool PlaceShip(int shipIndex, int length)
        {
            bool horizontal = rnd.Next(2) == 0;

            int startX = rnd.Next(Size - length);
            int startY = rnd.Next(Size);

            int endX = horizontal ? startX + length : startX;
            int endY = horizontal ? startY : startY + length;

            if (!IsPlacementValid(startX, startY, endX, endY))
            {
                return false;
            }

            for (int i = 0; i < length; ++i)
            {
                SeaCell square = horizontal
                    ? GetCellInternal(startX + i, startY)
                    : GetCellInternal(startX, startY + i);

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
                if (!IsSquareValid(i,y) || !IsSquareFree(i, y))
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
            if(index < 0 || index >= ships.Count)
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
