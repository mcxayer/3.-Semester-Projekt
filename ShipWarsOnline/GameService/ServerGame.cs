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
            game = new LocalGame(null,null);
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

            var grids = game.ReadOnlyGrids;
            var opponentCells = grids[(playerIndex + 1) % 2].ReadOnlyCells;

            // NOTE: Jagged arrays are necessary, as WCF does not like multidimensional arrays
            SeaCellData[][] opponentCellsData = DTOFactory.CreateCellData(opponentCells);

            for (int i = 0; i < opponentCellsData.Length; i++)
            {
                for (int j = 0; j < opponentCellsData[i].Length; j++)
                {
                    SeaCellData cellData = opponentCellsData[i][j];

                    if (!cellData.Revealed)
                    {
                        cellData.Type = CellType.Unknown;
                        cellData.ShipIndex = -1;
                    }
                }
            }

            GameStateDTO state = new GameStateDTO();
            state.PlayerGrid = DTOFactory.CreateSeaGridData(grids[playerIndex]);
            state.OpponentCells = opponentCellsData;
            state.PlayerIndex = playerIndex;

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
