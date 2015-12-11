﻿using System;
using System.ServiceModel;
using System.Collections.Generic;
using GameService;
using ShipWarsOnline.Data;

namespace Client
{
    // Flyt alt over til client
    public class Application
    {
        static void Main(string[] args)
        {
            var generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            //channelfactory.Credentials.UserName.UserName = "username";
            //channelfactory.Credentials.UserName.Password = "password";
            var generalProxy = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<GameService.IService>(new DuplexedClass(),"GameServiceEndpoint");
            var gameProxy = gameFactory.CreateChannel();

            Console.WriteLine("Dette er Klienten");

            try
            {
                generalProxy.CreateAccount("hej", "med", "dig");
                generalProxy.CreateAccount("mit", "navn", "er");
                generalProxy.CreateAccount("kaj", "hvad", "hedder");
                generalProxy.CreateAccount("du", "bob", "bobsen");
                Console.WriteLine("Ny bruger skabt!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke skabe ny bruger!");
            }

            String tokenID = null;
            try
            {
                tokenID = generalProxy.Login("hej", "med");
                Console.WriteLine("Logget ind med token id: " + tokenID);

                //if (gameProxy.Connect(tokenID))
                //{
                //    Console.WriteLine("Forbundet til spilserver!");
                //    gameProxy.Matchmake();
                //}
            }
            catch (FaultException)
            {
                Console.WriteLine("Kunne ikke logge ind!");
            }
            catch (CommunicationException ex)
            {
                Console.WriteLine(ex.ToString());
            }


            try  // Print lobby to console
            {
                List<string> lobby = gameProxy.GetLobby();
                foreach (string s in lobby)
                {
                    Console.WriteLine(s);
                }

            }
            catch
            {
                Console.WriteLine("Could not retrieve lobby!");
            }


            Console.WriteLine("\nTryk Enter for at afslutte session...");
            Console.ReadLine();

            try
            {
                //if (gameProxy.Disconnect())
                //{
                //    Console.WriteLine("Afsluttet forbindelse til spilserver!");
                //}
                //else
                //{
                //    Console.WriteLine("Kunne ikke afslutte forbindelse til spilserver!");
                //}

                generalProxy.Logout(tokenID);
                Console.WriteLine("Logget ud!");
            }
            catch
            {
                Console.WriteLine("Kunne ikke logge ud!");
            }

            Console.Read();
        }

        private class DuplexedClass : GameService.ICallback
        {
            public void OnLobbyUpdated()
            {
                Console.WriteLine("Lobby updated!");
            }

            public void OnPlayerConnected()
            {
                Console.WriteLine("Player connected to the game server!");
            }

            public void OnPlayerDisconnected()
            {
                Console.WriteLine("Player disconnected from the game server!");
            }

            public void OnPlayerFailedConnecting()
            {
                throw new NotImplementedException();
            }

            public void OnPlayerEnteredMatchmaking()
            {
                Console.WriteLine("Player entered matchmaking!");
            }

            public void OnPlayerCancelledMatchmaking()
            {
                Console.WriteLine("Player exited matchmaking!");
            }

            public void OnPlayerMatchmade()
            {
                Console.WriteLine("Player matchmade to game!");
            }

            public void OnGameInit(GameInitStateDTO initState)
            {
                throw new NotImplementedException();
            }

            public void OnCellImpact(GameCellImpactDTO cellImpact)
            {
                throw new NotImplementedException();
            }

            public void OnShipRevealed(ShipData ship)
            {
                throw new NotImplementedException();
            }
        }
    }

}