using Game.ReadOnly;

namespace Client.GUI
{
    public interface IGUISeaGrid
    {
        ReadOnly2DArray<ReadOnlySeaCell> GridCells { get; set; }
        ICellSelectionListener CellSelectionCallback { get; set; }
        void UpdateGrid();
    }
}
