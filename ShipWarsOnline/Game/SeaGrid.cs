using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public class SeaGrid
    {
        private static readonly int MaxLoopCount = 10000;
        private static readonly int DefaultGridSize = 10;

        private SeaSquare[,] cells;
        private int Size { get; set; }

        private List<Ship> ships = new List<Ship>();
        private Random rnd = new Random();

        public SeaGrid() : this(DefaultGridSize) { }
        public SeaGrid(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "size must be greater than zero!");
            }
            Size = size;

            cells = new SeaSquare[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    cells[i, j] = new SeaSquare();
                }
            }

            ships = new List<Ship>();

            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                ships.Add(new Ship(type));
            }

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
            cells = new SeaSquare[Size, Size];
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i,j] = new SeaSquare(data.Cells[i][j]);
                }
            }

            ships = new List<Ship>();
            for (int i = 0; i < data.Ships.Length; i++)
            {
                ships.Add(new Ship(data.Ships[i]));
            }
        }

        public void Reset()
        {
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    SeaSquare square = GetCell(i, j);
                    square.Type = SquareType.Water;
                    square.ShipIndex = -1;
                }
            }

            PlaceShips();
        }

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
                SeaSquare square = horizontal
                    ? GetCell(startX + i, startY)
                    : GetCell(startX, startY + i);

                square.Type = SquareType.Undamaged;
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
            return GetCell(x, y).ShipIndex == -1;
        }
       
        private bool IsSquareValid(int x, int y)
        {
            return x >= 0 && x < Size && y >= 0 && y < Size;
        }

        public SquareType FireAt(int x, int y)
        {
            SeaSquare square = GetCell(x, y);

            if(square.Type == SquareType.Undamaged)
            {
                int indexToDamage = square.ShipIndex;
                Ship ship = GetShip(indexToDamage);
                ship.Damage();

                if(ship.Sunk)
                {
                    SinkShip(indexToDamage);
                }
                else
                {
                    square.Type = SquareType.Damaged;
                }
            }

            return square.Type;
        }

        private Ship GetShip(int index)
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
                    SeaSquare square = GetCell(i, j);
                    if (square.ShipIndex == shipIndex)
                    {
                        square.Type = SquareType.Sunk;
                    }
                }
            }
        }

        public bool AreAllShipsSunk()
        {
            return ships.All(ship => ship.Sunk);
        }

        private SeaSquare GetCell(int x, int y)
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

        public SeaGridData GetData()
        {
            return new SeaGridData()
            {
                Ships = GetShipData(),
                Cells = GetCellData()
            };
        }

        public SeaSquareData[][] GetCellData()
        {
            SeaSquareData[][] data = new SeaSquareData[Size][];
            for (int i = 0; i < Size; i++)
            {
                data[i] = new SeaSquareData[Size];

                for (int j = 0; j < Size; j++)
                {
                    data[i][j] = cells[i,j].GetData();
                }
            }

            return data;
        }

        public ShipData[] GetShipData()
        {
            ShipData[] data = new ShipData[ships.Count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = ships[i].GetData();
            }
            return data;
        }
    }
}
