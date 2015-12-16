using Game.ReadOnly;
using GameData;
using System;
using System.Collections.Generic;

namespace GameServices
{
    public static class DTOFactory
    {
        public static SeaCellData CreateCellData(ReadOnlySeaCell cell)
        {
            if(cell == null)
            {
                throw new ArgumentNullException("cell");
            }

            return new SeaCellData
            {
                ShipIndex = cell.ShipIndex,
                Type = cell.Type,
                Revealed = cell.Revealed
            };
        }

        public static ShipData CreateShipData(ReadOnlyShip ship)
        {
            if (ship == null)
            {
                throw new ArgumentNullException("ship");
            }

            return new ShipData
            {
                Type = ship.Type,
                Health = ship.Health
            };
        }

        public static SeaCellData[][] CreateCellData(ReadOnly2DArray<ReadOnlySeaCell> cells)
        {
            if (cells == null)
            {
                throw new ArgumentNullException("cells");
            }

            SeaCellData[][] data = new SeaCellData[cells.GetLength(0)][];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new SeaCellData[cells.GetLength(1)];

                for (int j = 0; j < data[i].Length; j++)
                {
                    data[i][j] = CreateCellData(cells[i, j]);
                }
            }

            return data;
        }

        public static ShipData[] CreateShipData(IReadOnlyList<ReadOnlyShip> ships)
        {
            if (ships == null)
            {
                throw new ArgumentNullException("ships");
            }

            ShipData[] data = new ShipData[ships.Count];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = CreateShipData(ships[i]);
            }
            return data;
        }

        public static SeaGridData CreateSeaGridData(ReadOnlySeaGrid grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            return new SeaGridData()
            {
                Ships = CreateShipData(grid.ReadOnlyShips),
                Cells = CreateCellData(grid.ReadOnlyCells)
            };
        }
    }
}
