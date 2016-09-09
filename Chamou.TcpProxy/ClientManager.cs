using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chamou.TcpProxy
{
    public class ClientManager
    {
        private Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();
        private static ClientManager _instance;

        private ClientManager() { }

        public static ClientManager Instance =>
            _instance ?? (_instance = new ClientManager());

        internal void AddClient(TcpClient client, int clientId)
        {
            if (clients.ContainsKey(clientId))
            {
                var oldClient = clients[clientId];
                oldClient.Close();
            }
            clients[clientId] = client;
        }

        public bool IsClientAvailable(int clientId) =>
            clients.ContainsKey(clientId) && clients[clientId].Connected;

        public bool SendMessage(int clientId, string data)
        {
            var client = clients[clientId];
            if (client != null && client.Connected)
            {
                var stream = client.GetStream();
                var bytes = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(bytes, 0, bytes.Length);

                return true;
            }

            return false;
        }


    }
}
