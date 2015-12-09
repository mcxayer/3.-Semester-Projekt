using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Battleship.Game
{
    /// <summary>
    /// Interaction logic for SeaGridControl.xaml
    /// </summary>
    public partial class SeaGridControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<List<SeaSquare>> gridCells;
        public List<List<SeaSquare>> GridCells
        {
            get { return gridCells; }
            set { gridCells = value; OnPropertyChanged("GridCells"); }
        }

        public SeaGridControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void SetCells(List<List<SeaSquare>> gridCells)
        {
            //this.gridCells = gridCells;

            //this.gridCells.Clear();
            //for (int i = 0; i < gridCells.Count; i++)
            //{
            //    this.gridCells.Add(new ObservableCollection<SeaSquare>());
            //    foreach (var cell in gridCells[i])
            //    {
            //        this.gridCells[i].Add(cell);
            //    }
            //}

            //for (int i = 0; i < gridCells.Count; i++)
            //{
            //    gridControl.ColumnDefinitions.Add(new ColumnDefinition());
            //    gridControl.RowDefinitions.Add(new RowDefinition());
            //}

            //for (int i = 0; i < gridCells.Count; i++)
            //{
            //    for (int j = 0; j < gridCells[i].Count; j++)
            //    {
            //        Rectangle rct = new Rectangle();
            //        rct.Fill = new SolidColorBrush(Colors.Black);
            //        rct.Width = 10;
            //        rct.Height = 10;
            //        Grid.SetRow(rct, j);
            //        Grid.SetColumn(rct, i);

            //        gridControl.Children.Add(rct);
            //    }
            //}
        }

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnCellClicked(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem item = sender as ListBoxItem;
            SeaSquare content = item.Content as SeaSquare;

            //NOTE: sometimes if you click really fast you can end up clicking on what the debugger says is a "ListBoxItem {DisconnectedItem}
            //hunting down the exact cause would take ages, and might even be a bug in WPF or something
            if (content == null)
                return;

            GameContextFacade.Instance.TakeTurn(content.Row, content.Col);
        }
    }
}
