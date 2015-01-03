using System;
using DarkNetwork;
using MessageStream2;

namespace SyncAudioServer
{
    public class NetworkMessages
    {
        public static void HandleConnectCallback(NetworkClient<ClientObject> client)
        {
            client.stateObject = new ClientObject();
            client.stateObject.networkClient = client;
        }

        public static void HandleSyncTime(ClientObject client, byte[] messageData)
        {
            client.syncNumber++;
            if (client.syncNumber < 10)
            {
                Console.WriteLine("Syncing client ("+client.syncNumber+"/10)");
            }
            else
            {
                Console.WriteLine("Client synced!");
            }

            NetworkMessage newMessage = new NetworkMessage(0);
            using (MessageReader mr = new MessageReader(messageData))
            {
                using (MessageWriter mw = new MessageWriter())
                {
                    mw.Write<long>(mr.Read<long>());
                    mw.Write<long>(DateTime.UtcNow.Ticks);
                    newMessage.messageData = mw.GetMessageBytes();
                }
            }
            client.networkClient.QueueMessage(newMessage);
        }

        public static void SendPlay(NetworkServer<ClientObject> networkServer)
        {
            NetworkClient<ClientObject>[] netClients = networkServer.GetClients();
            Console.WriteLine("Playing audio on " + netClients.Length + " clients!");
            NetworkMessage newMessage = new NetworkMessage(1);
            using (MessageWriter mw = new MessageWriter())
            {
                mw.Write<long>(DateTime.UtcNow.Ticks);
                newMessage.messageData = mw.GetMessageBytes();
            }
            foreach (NetworkClient<ClientObject> client in netClients)
            {
                client.QueueMessage(newMessage);
            }
        }

        public static void SendStop(NetworkServer<ClientObject> networkServer)
        {
            NetworkClient<ClientObject>[] netClients = networkServer.GetClients();
            Console.WriteLine("Stopping audio on " + netClients.Length + " clients!");
            NetworkMessage newMessage = new NetworkMessage(2);
            foreach (NetworkClient<ClientObject> client in netClients)
            {
                client.QueueMessage(newMessage);
            }
        }
    }
}

