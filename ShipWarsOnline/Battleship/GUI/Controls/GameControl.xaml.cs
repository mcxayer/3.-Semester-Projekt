using Battleship.Game;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System;
using Battleship.GUI;
using ShipWarsOnline;
using System.Collections.ObjectModel;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl, IGUIGame, ICellSelectionListener
    {
        private ReadOnlySeaCell selectedCell;
        private IGUISeaGrid playerGrid;
        private IGUISeaGrid opponentGrid;

        public GameControl()
        {
            InitializeComponent();

            playerGrid = myGrid;
            opponentGrid = enemyGrid;

            playerGrid.CellSelectionCallback = this;
            opponentGrid.CellSelectionCallback = this;
        }

        private void OnEndTurnButtonClicked(object sender, RoutedEventArgs e)
        {
            if(selectedCell == null)
            {
                throw new Exception("No selected cell!");
            }

            GUIFacade.Instance.TakeTurn(selectedCell.PosX, selectedCell.PosY);
        }

        public void OnGameInit()
        {
            playerGrid.GridCells = GUIFacade.Instance.GetPlayerCells();
            opponentGrid.GridCells = GUIFacade.Instance.GetOpponentCells();

            lblUsername1.Content = GUIFacade.Instance.GetPlayerName();
            lblUsername2.Content = GUIFacade.Instance.GetOpponentName();

            UpdateEnabled();
            UpdateOverlay();
        }

        private void Reset()
        {
            selectedCell = null;
            btnEndTurn.IsEnabled = false;
            UpdateEnabled();
            UpdateOverlay();
        }

        public void OnSelected()
        {
            Reset();
        }

        public void OnCellSelected(object source, ReadOnlySeaCell cell)
        {
            if(source != myGrid && source != enemyGrid)
            {
                throw new Exception("Invalid source!");
            }

            selectedCell = cell;
            if(!selectedCell.Revealed)
            {
                lblType.Content = "Unknown";
                btnEndTurn.IsEnabled = (source == enemyGrid);
            }
            else
            {
                lblType.Content = selectedCell.Type.ToString();
            }

            lblPos.Content = string.Format("X: {0}, Y: {1}", selectedCell.PosX, selectedCell.PosY);
        }

        public void OnTurnTaken()
        {
            UpdateGrids();

            Reset();
        }

        public void OnShipDestroyed()
        {
            UpdateGrids();
        }

        public void OnPlayerWon()
        {
            EnableOverlay(true);
            lblOverlayInfo.Content = "You have won!";
            EnableControl(false);
        }

        public void OnPlayerLost()
        {
            EnableOverlay(true);
            lblOverlayInfo.Content = "You have lost!";
            EnableControl(false);
        }

        public FrameworkElement GetElement()
        {
            return this;
        }

        private void UpdateEnabled()
        {
            EnableControl(GUIFacade.Instance.IsPlayerTurn());
        }

        private void UpdateGrids()
        {
            playerGrid.UpdateGrid();
            opponentGrid.UpdateGrid();
        }

        private void EnableControl(bool enable)
        {
            gridContent.IsEnabled = enable;
            gridFooter.IsEnabled = enable;
        }

        private void UpdateOverlay()
        {
            EnableOverlay(!GUIFacade.Instance.IsPlayerTurn());
        }

        private void EnableOverlay(bool enable)
        {
            gridOverlay.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
