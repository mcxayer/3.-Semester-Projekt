using Game.ReadOnly;
using GameData;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.GUI
{
    [ValueConversion(typeof(CellType), typeof(Brush))]
    public class ColorController: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlySeaCell cell = value as ReadOnlySeaCell;
            if(cell == null)
            {
                throw new Exception(string.Format("Object of type {0} is not valid for conversion!",value.GetType()));
            }

            if(!cell.Revealed)
            {
                return new SolidColorBrush(Colors.LightGray);
            }

            switch (cell.Type)
            {
                case CellType.Water:
                    return new SolidColorBrush(Colors.LightBlue);
                case CellType.Undamaged:
                    return new SolidColorBrush(Colors.Black);
                case CellType.Damaged:
                    return new SolidColorBrush(Colors.Orange);
                case CellType.Sunk:
                    return new SolidColorBrush(Colors.Red);
            }

            throw new Exception("Failed to convert cell!");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
