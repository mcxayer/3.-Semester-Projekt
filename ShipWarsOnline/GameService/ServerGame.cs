using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.ServiceModel;

namespace GameService
{
    public class ServerGame
    {
        private LocalGame game;
        private IContextChannel[] playerChannels;

        public ServerGame(IContextChannel playerChannel1, IContextChannel playerChannel2)
        {
            game = new LocalGame(null, null);

            // Setup for a scenario with all the ship types
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            {
                game.AddShip(type);
            }

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

        public GameInitStateDTO GetInitGameState(IContextChannel playerChannel)
        {
            int playerIndex = GetPlayerIndex(playerChannel);
            if (playerIndex == -1)
            {
                throw new Exception("Could not get game state as player does not exist in game!");
            }

            var playerShips = game.ReadOnlyGrids[playerIndex].ReadOnlyShips;
            ShipType[] ships = new ShipType[playerShips.Count];

            for (int i = 0; i < ships.Length; i++)
            {
                ships[i] = playerShips[i].Type;
            }

            return new GameInitStateDTO()
            {
                GridSize = game.ReadOnlyGrids[0].Size,
                Ships = ships,
                PlayerIndex = playerIndex
            };
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
