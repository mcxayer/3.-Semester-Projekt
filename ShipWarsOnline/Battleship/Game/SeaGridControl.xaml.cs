using Battleship.GUI;
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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
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

            GUIFacade.Instance.TakeTurn(content.Row, content.Col);
        }
    }
}
