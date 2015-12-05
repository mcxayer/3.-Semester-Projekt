using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    [Serializable]
    public class SeaGrid : Grid<SeaSquare>
    {
        private static readonly int MaxLoopCount = 10000;

        private List<Ship> ships = new List<Ship>();
        private Random rnd = new Random();

        public SeaGrid() : this(DefaultGridSize) { }
        public SeaGrid(int size) : base(size)
        {
            ships = new List<Ship>();

            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                ships.Add(new Ship(type));
            }

            Reset();
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
    }
}
