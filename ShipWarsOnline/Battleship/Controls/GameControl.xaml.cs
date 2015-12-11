using Battleship.Game;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using Battleship.GUI;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl, IGUIGame, ICellSelectionListener
    {
        private SeaSquare selectedCell;

        public GameControl()
        {
            InitializeComponent();

            myGrid.CellSelectionCallback = this;
            enemyGrid.CellSelectionCallback = this;
        }

        private void OnEndTurnButtonClicked(object sender, RoutedEventArgs e)
        {
            if(selectedCell == null)
            {
                throw new Exception("No selected cell!");
            }

            GUIFacade.Instance.TakeTurn(selectedCell.Row, selectedCell.Col);
        }

        public void OnGameInit()
        {
            var roCellsPlayer = GUIFacade.Instance.GetPlayerCells();
            List<List<SeaSquare>> cellsPlayer = new List<List<SeaSquare>>();
            for (int i = 0; i < roCellsPlayer.GetLength(0); i++)
            {
                cellsPlayer.Add(new List<SeaSquare>());

                for (int j = 0; j < roCellsPlayer.GetLength(1); j++)
                {
                    SeaSquare square = new SeaSquare(i, j);
                    square.Reset(roCellsPlayer[i, j].Type);
                    cellsPlayer[i].Add(square);
                }
            }

            var roCellsOpponent = GUIFacade.Instance.GetOpponentCells();
            List<List<SeaSquare>> cellsOpponent = new List<List<SeaSquare>>();
            for (int i = 0; i < roCellsOpponent.GetLength(0); i++)
            {
                cellsOpponent.Add(new List<SeaSquare>());

                for (int j = 0; j < roCellsOpponent.GetLength(1); j++)
                {
                    SeaSquare square = new SeaSquare(i, j);
                    square.Reset(roCellsOpponent[i, j].Type);
                    cellsOpponent[i].Add(square);
                }
            }

            myGrid.GridCells = cellsPlayer;
            enemyGrid.GridCells = cellsOpponent;
        }

        public void OnSelected()
        {
            // Nothing
        }

        public void OnCellSelected(SeaSquare cell)
        {
            selectedCell = cell;

            lblType.Content = cell.Type.ToString();
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
