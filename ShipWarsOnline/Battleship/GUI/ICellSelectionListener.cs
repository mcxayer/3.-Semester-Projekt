using Game.ReadOnly;

namespace Client.GUI
{
    public interface ICellSelectionListener
    {
        void OnCellSelected(object source, ReadOnlySeaCell cell);
    }
}
