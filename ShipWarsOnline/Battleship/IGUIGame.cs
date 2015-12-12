using Battleship.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public interface IGUIGame : IGUIControl
    {
        void OnGameInit();
        void OnTurnTaken();

        void OnPlayerWon();

        void OnPlayerLost();
    }
}
