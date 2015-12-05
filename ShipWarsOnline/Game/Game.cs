using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public class Game
    {
        private static readonly int MaxPlayerAmount = 2;

        private IPlayer[] players;
        private SeaGrid[] grids;

        public int CurrentPlayerTurn { get; private set; }

        public Game(IPlayer player1, IPlayer player2)
        {
            players = new IPlayer[MaxPlayerAmount];
            players[0] = player1;
            players[1] = player2;

            grids = new SeaGrid[MaxPlayerAmount];
            for (int i = 0; i < MaxPlayerAmount; i++)
            {
                grids[i] = new SeaGrid();
            }
        }

        public void TakeTurn(int x, int y)
        {
            int nextPlayerIndex = (CurrentPlayerTurn + 1) % MaxPlayerAmount;
            grids[nextPlayerIndex].FireAt(x,y);

            CurrentPlayerTurn = nextPlayerIndex;
        }

        public SeaGrid GetGrid()
        {
            return grids[0];
        }
    }
}
