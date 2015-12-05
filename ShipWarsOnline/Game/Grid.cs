using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ShipWarsOnline
{
    [Serializable]
    public class Grid<T> where T : new()
    {
        protected static readonly int DefaultGridSize = 10;

        private T[,] cells;
        protected int Size { get; private set; }

        public Grid() : this(DefaultGridSize) { }
        public Grid(int size)
        {
            if(size <= 0)
            {
                throw new ArgumentOutOfRangeException("size", "size must be greater than zero!");
            }
            Size = size;

            cells = new T[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    cells[i, j] = new T();
                }
            }
        }

        public T GetCell(int x, int y)
        {
            if(x < 0 || x >= Size)
            {
                throw new ArgumentOutOfRangeException("x");
            }

            if (y < 0 || y >= Size)
            {
                throw new ArgumentOutOfRangeException("y");
            }

            return cells[x, y];
        }
    }
}

