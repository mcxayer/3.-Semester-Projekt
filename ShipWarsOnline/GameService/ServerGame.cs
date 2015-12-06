using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GameService
{
    public class ServerGame
    {
        private Game game;
        private IContextChannel[] playerChannels;

        public ServerGame(IContextChannel playerChannel1, IContextChannel playerChannel2)
        {
            game = new Game(null,null);
            playerChannels = new IContextChannel[] { playerChannel1, playerChannel2 };
        }

        public void TakeTurn(int x, int y, IContextChannel playerChannel)
        {
            int playerIndex = GetPlayerIndex(playerChannel);
            if (playerIndex == -1)
            {
                throw new ArgumentException("playerChannel is not in game!", "playerChannel");
            }

            if (playerIndex != game.CurrentPlayerTurn)
            {
                throw new Exception("Not players turn!");
            }

            game.TakeTurn(x, y);
        }

        public GameStateDTO GetGameState(IContextChannel playerChannel)
        {
            int playerIndex = GetPlayerIndex(playerChannel);
            if ( playerIndex == -1)
            {
                throw new Exception("Could not get game state as player does not exist in game!");
            }

            SeaGridData[] grids = game.GetData();
            SeaSquareData[][] opponentCells = grids[(playerIndex + 1) % 2].Cells;

            for (int i = 0; i < opponentCells.Length; i++)
            {
                for (int j = 0; j < opponentCells[i].Length; j++)
                {
                    SeaSquareData square = opponentCells[i][j];

                    if(!square.Revealed)
                    {
                        square.Type = SquareType.Unknown;
                        square.ShipIndex = -1;
                    }
                }
            }

            GameStateDTO state = new GameStateDTO();
            state.playerGrid = grids[playerIndex];
            state.opponentCells = opponentCells;

            return state;
        }

        private int GetPlayerIndex(IContextChannel playerChannel)
        {
            int playerIndex = -1;
            if (playerChannel == playerChannels[0])
            {
                playerIndex = 0;
            }
            else if (playerChannel == playerChannels[1])
            {
                playerIndex = 1;
            }

            return playerIndex;
        }
    }
}
