using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;

namespace Chamou.Web.Tcp
{
    public class TcpManager
    {
        private Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();

        internal TcpManager() {  }

        public static TcpManager Instance => 
            HttpContext.Current.Application[TcpManagerConfig.AppKey] as TcpManager;

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