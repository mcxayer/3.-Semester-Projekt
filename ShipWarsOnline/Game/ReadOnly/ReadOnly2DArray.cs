using System;

namespace Game.ReadOnly
{
    /// <summary>
    /// Read-only wrapper of a generic 2d array to be used outside the game context
    /// </summary>
    public class ReadOnly2DArray<T>
    {
        private T[,] array;

        public ReadOnly2DArray(T[,] array)
        {
            if(array == null)
            {
                throw new ArgumentNullException("array");
            }

            this.array = array;
        }

        public T GetIndex(int x, int y)
        {
            if(x < 0 || x >= array.GetLength(0))
            {
                throw new ArgumentOutOfRangeException("x");
            }

            if (y < 0 || y >= array.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("y");
            }

            return array[x, y];
        }

        public int GetLength(int dimension)
        {
            return array.GetLength(dimension);
        }

        public T this[int x, int y]
        {
            get { return array[x, y]; }
        }
    }
}
