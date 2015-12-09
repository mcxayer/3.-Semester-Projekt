using Battleship.Game;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : UserControl
    {
        private MainWindow window;

        public GameWindow()
        {
            InitializeComponent();

            GameContextFacade.Instance.HandleGameInitialized += OnGameInit;
        }

        ~GameWindow()
        {
            try
            {
                GameContextFacade.Instance.HandleGameInitialized -= OnGameInit;
            }
            catch
            {
                // Nothing
            }
        }

        public void SetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void OnGameInit()
        {
            var cells = GameContextFacade.Instance.GetPlayerCells();
            List<List<SeaSquare>> gridCells = new List<List<SeaSquare>>();
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                gridCells.Add(new List<SeaSquare>());

                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    SeaSquare square = new SeaSquare(i, j);
                    square.Reset(cells[i, j].Type);
                    gridCells[i].Add(square);
                }
            }

            myGrid.GridCells = gridCells;
        }
    }
}
