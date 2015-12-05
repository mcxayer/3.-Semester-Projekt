using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Battleship.Game;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Battleship.Game
{
    /// <summary>
    /// Interaction logic for ComputerGrid.xaml
    /// </summary>
    public partial class SeaGrid : UserControl
    {
        public SeaGrid()
        {
            InitializeComponent();
        }

        private void Item_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid vm = this.DataContext as Grid;
            ListBoxItem item = sender as ListBoxItem;
            SeaSquare content = item.Content as SeaSquare;

            ShipWarsOnline.SeaGrid grid;
            byte[] gridBytes = ServiceFacade.Instance.GetGameState().grid;

            if(gridBytes == null)
            {
                Console.WriteLine("grid bytes is null!");
                return;
            }
            using (MemoryStream stream = new MemoryStream(gridBytes))
            {
                grid = (ShipWarsOnline.SeaGrid)new BinaryFormatter().Deserialize(stream);
            }

            if(grid == null)
            {
                Console.WriteLine("Game is null!");
            }
            else
            {
                Console.WriteLine(grid.GetCell(0,0));
            }

            //XXX sometimes if you click really fast you can end up clicking on what the debugger says is a "ListBoxItem {DisconnectedItem}
            //hunting down the exact cause would take ages, and might even be a bug in WPF or something
            if (content == null)
                return;

            vm.Clicked(content);            
        }
    }
}
