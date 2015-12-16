using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Windows;
using Game.ReadOnly;
using GameData;

namespace Client.GUI.Controls
{
    /// <summary>
    /// Interaction logic for SeaGridControl.xaml
    /// </summary>
    public partial class SeaGridControl : UserControl, IGUISeaGrid, IDisposable
    {
        private ReadOnly2DArray<ReadOnlySeaCell> gridCells;
        public ReadOnly2DArray<ReadOnlySeaCell> GridCells
        {
            get { return gridCells; }
            set { gridCells = value; CreateGrid(); }
        }

        public ICellSelectionListener CellSelectionCallback { get; set; }

        private SeaCellControl[,] GridCellControls;

        public SeaGridControl()
        {
            InitializeComponent();

            DataContext = this;
        }

        public void Dispose()
        {
            CleanupGrid();
        }

        private void CreateGrid()
        {
            CleanupGrid();

            RowDefinition[] rows = new RowDefinition[GridCells.GetLength(0)];
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = new RowDefinition();
                rows[i].Height = GridLength.Auto;
                gridControl.RowDefinitions.Add(rows[i]);
            }

            ColumnDefinition[] columns = new ColumnDefinition[GridCells.GetLength(1)];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = new ColumnDefinition();
                columns[i].Width = GridLength.Auto;
                gridControl.ColumnDefinitions.Add(columns[i]);
            }

            DataTemplate dataTemplate = FindResource("CellTemplate") as DataTemplate;
            if(dataTemplate == null)
            {
                throw new Exception("No cell template found!");
            }

            GridCellControls = new SeaCellControl[rows.Length,columns.Length];
            for (int i = 0; i < GridCells.GetLength(0); i++)
            {
                for (int j = 0; j < GridCells.GetLength(1); j++)
                {
                    SeaCellControl cellControl = new SeaCellControl();
                    cellControl.Cell = GridCells[j, (GridCells.GetLength(0) - 1) - i];
                    cellControl.rectCell.Fill = GetCellBrush(cellControl.Cell);
                    cellControl.MouseLeftButtonUp += OnCellClicked;
                    GridCellControls[i, j] = cellControl;

                    gridControl.Children.Add(cellControl);

                    Grid.SetRow(cellControl, i);
                    Grid.SetColumn(cellControl, j);
                }
            }
        }

        public void UpdateGrid()
        {
            if(GridCellControls == null)
            {
                return;
            }

            for (int i = 0; i < GridCellControls.GetLength(0); i++)
            {
                for (int j = 0; j < GridCellControls.GetLength(1); j++)
                {
                    GridCellControls[i, j].rectCell.Fill = GetCellBrush(GridCells[j,(GridCells.GetLength(0) - 1) - i]);
                }
            }
        }

        private void CleanupGrid()
        {
            if (GridCellControls != null)
            {
                for (int i = 0; i < GridCellControls.GetLength(0); i++)
                {
                    for (int j = 0; j < GridCellControls.GetLength(1); j++)
                    {
                        GridCellControls[i,j].MouseLeftButtonUp -= OnCellClicked;
                    }
                }
            }

            gridControl.ColumnDefinitions.Clear();
            gridControl.RowDefinitions.Clear();
        }

        private Brush GetCellBrush(ReadOnlySeaCell cell)
        {
            if (!cell.Revealed)
            {
                return new SolidColorBrush(Colors.LightGray);
            }

            switch (cell.Type)
            {
                case CellType.Water:
                    return Brushes.LightBlue;
                case CellType.Undamaged:
                    return Brushes.Black;
                case CellType.Damaged:
                    return Brushes.Orange;
                case CellType.Sunk:
                    return Brushes.Red;
            }

            return null;
        }

        private void OnCellClicked(object sender, MouseButtonEventArgs e)
        {
            SeaCellControl cellControl = sender as SeaCellControl;
            if (cellControl == null)
            {
                return;
            }

            if (CellSelectionCallback != null)
            {
                CellSelectionCallback.OnCellSelected(this, cellControl.Cell);
            }
        }
    }
}
