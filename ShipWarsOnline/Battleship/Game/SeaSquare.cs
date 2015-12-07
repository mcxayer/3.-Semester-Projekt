using ShipWarsOnline.Data;
using System.Windows;

namespace Battleship.Game
{
    public class SeaSquare : DependencyObject
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int ShipIndex { get; set; }

        public CellType Type
        {
            get { return (CellType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        public static readonly DependencyProperty TypeProperty =
        DependencyProperty.Register("Type", typeof(CellType), typeof(SeaSquare), null);

        public SeaSquare(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void Reset(CellType type)
        {
            Type = type;
            ShipIndex = -1;
        }
    }
}
