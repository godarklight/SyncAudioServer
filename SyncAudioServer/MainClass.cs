using System;
using System.Threading;
using DarkNetwork;


namespace SyncAudioServer
{
    public class MainClass
    {
        private static NetworkServer<ClientObject> networkServer;

        public static void Main()
        {
            SetupNetwork();
            Console.WriteLine("Ready for action!");
            while (true)
            {
                bool handled = false;
                string currentLine = Console.ReadLine();
                if (currentLine == "/quit" || currentLine == "/exit")
                {
                    handled = true;
                    break;
                }
                if (currentLine == "/play")
                {
                    handled = true;
                    NetworkMessages.SendPlay(networkServer);
                }
                if (currentLine == "/stop")
                {
                    handled = true;
                    NetworkMessages.SendPlay(networkServer);
                }
                if (!handled)
                {
                    Console.WriteLine("Please type /play, /stop, or /quit");
                }
            }
            Console.WriteLine("Shutting down...");
            ShutdownNetwork();
            Console.WriteLine("Goodbye!");
        }

        private static void SetupNetwork()
        {
            NetworkHandler<ClientObject> networkHandler = new NetworkHandler<ClientObject>();
            networkHandler.RegisterCallback(0, NetworkMessages.HandleSyncTime);
            networkServer = new NetworkServer<ClientObject>(networkHandler, NetworkMessages.HandleConnectCallback, 6700);
        }

        private static void ShutdownNetwork()
        {
            networkServer.Stop();
            Thread.Sleep(1000);
        }
    }
}

