using ShipWarsOnline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class GameContextFacade
    {
        private static readonly GameContextFacade instance = new GameContextFacade();
        public static GameContextFacade Instance { get { return instance; } }

        private ServiceFacade serviceFacade;
        private IGame game;

        private GameContextFacade()
        {
            serviceFacade = new ServiceFacade();
        }

        public void CreateNetworkGame()
        {
            game = new NetworkGame();
        }

        public void TakeTurn(int x, int y)
        {
            game.TakeTurn(x, y);
        }
    }
}
