using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public class ComputerPlayer : IPlayer
    {
        public SeaGrid Grid { get; set; }

        //public void TakeTurnAutomated(Player otherPlayer)
        //{
        //    bool takenShot = false;
        //    while (!takenShot)
        //    {
        //        int row = rnd.Next(GRID_SIZE);
        //        int col = rnd.Next(GRID_SIZE);

        //        if (EnemyGrid[row][col].Type == SquareType.Unknown)
        //        {
        //            Fire(row, col, otherPlayer);
        //            takenShot = true;
        //        }
        //    }
        //}
    }
}
