using ShipWarsOnline.Data;

namespace ShipWarsOnline
{
    /// <summary>
    /// Read-only wrapper of SeaCell to be used outside the game context
    /// </summary>
    public class ReadOnlySeaCell
    {
        private SeaCell cell;

        public int PosX { get { return cell.PosX; } }
        public int PosY { get { return cell.PosY; } }
        public int ShipIndex { get { return cell.ShipIndex; } }
        public CellType Type { get { return cell.Type; } }
        public bool Revealed { get { return cell.Revealed; } }

        public ReadOnlySeaCell(SeaCell cell)
        {
            this.cell = cell;
        }
    }
}
