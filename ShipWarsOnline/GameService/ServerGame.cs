using ShipWarsOnline;
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
            int playerIndex = -1;
            if(playerChannel == playerChannels[0])
            {
                playerIndex = 0;
            }
            else if (playerChannel == playerChannels[1])
            {
                playerIndex = 1;
            }

            if(playerIndex == -1)
            {
                throw new ArgumentException("playerChannel is not in game!", "playerChannel");
            }

            if(playerIndex != game.CurrentPlayerTurn)
            {
                throw new Exception("Not players turn!");
            }

            game.TakeTurn(x, y);
        }

        public GameStateDTO GetGameState(IContextChannel playerChannel)
        {
            if(playerChannel != playerChannels[0] && playerChannel != playerChannels[1])
            {
                throw new Exception("Could not get game state as player does not exist in game!");
            }

            byte[] gridBytes;
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, game.GetGrid());
                gridBytes = stream.ToArray();
            }

            Console.WriteLine(gridBytes);

            GameStateDTO state = new GameStateDTO();
            state.grid = gridBytes;
            return state;
            //PlayerGridDTO playerGrid = new PlayerGridDTO();
            //playerGrid.gridType = new int[game.]
        }
    }
}
