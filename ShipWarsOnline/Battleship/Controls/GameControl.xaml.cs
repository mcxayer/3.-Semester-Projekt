using Battleship.Game;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using Battleship.GUI;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameControl : UserControl, IGUIGame
    {
        public GameControl()
        {
            InitializeComponent();
        }

        public void OnGameInit()
        {
            var cells = GUIFacade.Instance.GetPlayerCells();
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

        public void OnSelected()
        {
            // Nothing
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
