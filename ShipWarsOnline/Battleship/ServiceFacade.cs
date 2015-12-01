using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Battleship
{
    public class ServiceFacade
    {
        private static readonly ServiceFacade instance = new ServiceFacade();
        public static ServiceFacade Instance { get { return instance; } }

        public event Action<string> HandlePlayerConnected;
        public event Action<string> HandlePlayerDisconnected;
        public event Action<string> HandlePlayerMatchmade;
        public event Action HandlePlayerEnteredMatchmaking;
        public event Action HandlePlayerExitedMatchmaking;

        private GeneralService.IService generalService;
        private GameService.IService gameService;

        private TcpClient client;
        private Dispatcher uiDispatcher;

        private ServiceFacade()
        {
            var generalFactory = new ChannelFactory<GeneralService.IService>("GeneralServiceEndpoint");
            generalService = generalFactory.CreateChannel();

            var gameFactory = new DuplexChannelFactory<GameService.IService>(new CallbackHandler(this), "GameServiceEndpoint");
            ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            {
                gameService = gameFactory.CreateChannel();
            }));

            uiDispatcher = Dispatcher.CurrentDispatcher;

            try
            {
                client = new TcpClient();
                client.Connect(Dns.GetHostEntry("localhost").AddressList[1], 40000);
                new Thread(new ThreadStart(Initialize));
            }
            catch(SocketException)
            {
                Console.WriteLine("Could not connect to server!");
            }
        }

        private void Initialize()
        {
            while(client.Connected)
            {
                
            }
        }

        public string Login(string username, string password)
        {
            return generalService.Login(username,password);
        }

        public void Logout(string tokenId)
        {
            generalService.Logout(tokenId);
        }

        public void CreateAccount(string username, string password, string email)
        {
            generalService.CreateAccount(username, password, email);
        }

        //[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
        private class CallbackHandler : GameService.ICallback
        {
            ServiceFacade facade;

            public CallbackHandler(ServiceFacade facade)
            {
                this.facade = facade;
            }

            public void OnPlayerConnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} connected to the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerConnected(player)));
                }
            }

            public void OnPlayerDisconnected(string player)
            {
                Console.WriteLine(string.Format("Player {0} disconnected from the game server!", player));
                if (facade.HandlePlayerConnected != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerDisconnected(player)));
                }
            }

            public void OnPlayerMatchmade(string gameId)
            {
                Console.WriteLine(string.Format("Matchmade to game {0}!", gameId));
                if (facade.HandlePlayerMatchmade != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerMatchmade(gameId)));
                }
            }

            public void OnPlayerEnteredMatchmaking()
            {
                Console.WriteLine("Player entered matchmaking!");
                if (facade.HandlePlayerEnteredMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerEnteredMatchmaking()));
                }
            }

            public void OnPlayerExitedMatchmaking()
            {
                Console.WriteLine("Player exited matchmaking!");
                if (facade.HandlePlayerExitedMatchmaking != null)
                {
                    facade.uiDispatcher.BeginInvoke(new Action(() => facade.HandlePlayerExitedMatchmaking()));
                }
            }
        }

        public List<string> GetLobby()
        {
            return gameService.GetLobby();
        }

        public bool Connect(string tokenId)
        {
            return gameService.Connect(tokenId);
        }

        public bool Disconnect()
        {
            return gameService.Disconnect();
        }

        public void Matchmake()
        {
            gameService.Matchmake();
        }
    }
}
