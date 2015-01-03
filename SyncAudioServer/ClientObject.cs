using System;
using DarkNetwork;

namespace SyncAudioServer
{
    public class ClientObject
    {
        public NetworkClient<ClientObject> networkClient;
        public int syncNumber = 0;
    }
}

