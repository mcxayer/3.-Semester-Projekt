using ShipWarsOnline.Data;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship.Game
{
    [ValueConversion(typeof(CellType), typeof(Brush))]
    public class ColorController: IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            CellType type = (CellType)value;

            switch (type)
            {
                case CellType.Unknown:
                    return new SolidColorBrush(Colors.LightGray);
                case CellType.Water:
                    return new SolidColorBrush(Colors.LightBlue);
                case CellType.Undamaged:
                    return new SolidColorBrush(Colors.Black);
                case CellType.Damaged:
                    return new SolidColorBrush(Colors.Orange);
                case CellType.Sunk:
                    return new SolidColorBrush(Colors.Red);
            }

            throw new Exception("fail");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
