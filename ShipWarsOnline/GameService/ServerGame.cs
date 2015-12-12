using ShipWarsOnline;
using ShipWarsOnline.Data;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace GameService
{
    public class ServerGame
    {
        private LocalGame game;
        private IContextChannel[] playerChannels;
        public IReadOnlyList<IContextChannel> ReadOnlyPlayerChannels { get; private set; }

        private GameCellImpactDTO cellImpactCache;
        private GameShipDestroyedDTO destroyedShipCache;
        private bool shipDestroyed;

        public ServerGame(IContextChannel playerChannel1, IContextChannel playerChannel2)
        {
            game = new LocalGame(null, null);

            // Setup for a scenario with all the ship types
            //foreach (ShipType type in Enum.GetValues(typeof(ShipType)))
            //{
            //    game.AddShip(type);
            //}

            game.AddShip(ShipType.Submarine);

            playerChannels = new IContextChannel[] { playerChannel1, playerChannel2 };
            ReadOnlyPlayerChannels = new List<IContextChannel>(playerChannels).AsReadOnly();

            cellImpactCache = new GameCellImpactDTO();
            destroyedShipCache = new GameShipDestroyedDTO();
        }

        public void TakeTurn(int x, int y, IContextChannel playerChannel)
        {
            int playerIndex = GetPlayerIndex(playerChannel);
            if (playerIndex == -1)
            {
                throw new ArgumentException("Player is not in game!", "playerChannel");
            }

            if (playerIndex != game.CurrentPlayerTurn)
            {
                throw new Exception("Not player's turn!");
            }

            game.TakeTurn(x, y);

            int otherPlayerIndex = (playerIndex + 1) % 2;

            cellImpactCache.AffectedPosX = x;
            cellImpactCache.AffectedPosY = y;
            cellImpactCache.PlayerIndex = playerIndex;
            cellImpactCache.Type = game.ReadOnlyGrids[otherPlayerIndex].ReadOnlyCells[x, y].Type;

            shipDestroyed = false;

            Bounds destroyedShipBounds = game.GetDestroyedShip(otherPlayerIndex);
            if(destroyedShipBounds != null)
            {
                destroyedShipCache.StartPosX = destroyedShipBounds.MinX;
                destroyedShipCache.StartPosY = destroyedShipBounds.MaxY;
                destroyedShipCache.EndPosX = destroyedShipBounds.MaxX;
                destroyedShipCache.EndPosY = destroyedShipBounds.MinY;
                destroyedShipCache.PlayerIndex = playerIndex;

                shipDestroyed = true;
            }
        }

        public GameInitStateDTO GetInitGameState(IContextChannel playerChannel)
        {
            int playerIndex = GetPlayerIndex(playerChannel);
            if (playerIndex == -1)
            {
                throw new Exception("Could not get game state as player does not exist in game!");
            }

            var playerShips = game.ReadOnlyGrids[playerIndex].ReadOnlyShips;
            ShipData[] ships = new ShipData[playerShips.Count];

            for (int i = 0; i < ships.Length; i++)
            {
                ships[i] = new ShipData();
                ships[i].Type = playerShips[i].Type;
                ships[i].Health = playerShips[i].Health;
                ships[i].PosX = playerShips[i].PosX;
                ships[i].PosY = playerShips[i].PosY;
                ships[i].Horizontal = playerShips[i].Horizontal;
            }

            return new GameInitStateDTO()
            {
                GridSize = game.ReadOnlyGrids[0].Size,
                Ships = ships,
                PlayerIndex = playerIndex
            };
        }

        public GameCellImpactDTO GetCellImpact()
        {
            return new GameCellImpactDTO
            {
                AffectedPosX = cellImpactCache.AffectedPosX,
                AffectedPosY = cellImpactCache.AffectedPosY,
                PlayerIndex = cellImpactCache.PlayerIndex,
                Type = cellImpactCache.Type
            };
        }

        public GameShipDestroyedDTO GetDestroyedShip()
        {
            if (!shipDestroyed)
            {
                return null;
            }

            return new GameShipDestroyedDTO()
            {
                StartPosX = destroyedShipCache.StartPosX,
                StartPosY = destroyedShipCache.StartPosY,
                EndPosX = destroyedShipCache.EndPosX,
                EndPosY = destroyedShipCache.EndPosY,
                PlayerIndex = destroyedShipCache.PlayerIndex
            };
        }

        public int GetPlayerIndex(IContextChannel playerChannel)
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

        public bool IsGameOver()
        {
            return game.IsGameOver();
        }

        public int GetWinner()
        {
            return game.GetWinner();
        }
    }
}
