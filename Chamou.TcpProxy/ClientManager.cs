using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chamou.TcpProxy
{
    public class ClientManager
    {
        private ConcurrentDictionary<string, TcpClient> clients = new ConcurrentDictionary<string, TcpClient>();
        private static ClientManager _instance;

        private ClientManager() { }

        public static ClientManager Instance =>
            _instance ?? (_instance = new ClientManager());

        internal void AddClient(TcpClient client, string clientId)
        {
            if (clients.ContainsKey(clientId))
            {
                var oldClient = clients[clientId];
                oldClient.Close();
            }
            clients[clientId] = client;
        }

        public bool IsClientAvailable(string clientId) =>
            clients.ContainsKey(clientId) && clients[clientId].Connected;

        public bool SendMessage(string clientId, string data)
        {
            TcpClient client;
            if (clients.TryGetValue(clientId, out client))
            {
                if (client.Connected)
                {
                    var stream = client.GetStream();
                    var bytes = Encoding.ASCII.GetBytes(data);
                    stream.Write(bytes, 0, bytes.Length);

                    return true;
                }
            }
            return false;
        }


    }
}
