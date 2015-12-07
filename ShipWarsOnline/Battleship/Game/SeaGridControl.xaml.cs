using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Battleship.Game
{
    /// <summary>
    /// Interaction logic for ComputerGrid.xaml
    /// </summary>
    public partial class SeaGrid : UserControl
    {
        public List<List<SeaSquare>> GridCells { get; private set; }

        public SeaGrid()
        {
            InitializeComponent();

            var cells = GameContextFacade.Instance.GetPlayerCells();
            GridCells = new List<List<SeaSquare>>();
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                GridCells.Add(new List<SeaSquare>());

                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    SeaSquare square = new SeaSquare(i, j);
                    square.Type = cells[i, j].Type;
                    GridCells[i].Add(square);
                }
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
