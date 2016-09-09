using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace Chamou.Web.Tcp
{
    public class TcpManagerConfig
    {
        internal static string AppKey = "TCP_SERVER";
        public static void Initialize()
        {
            TcpManager tcpManager = HttpContext.Current.Application[AppKey] as TcpManager;
            if (tcpManager == null)
            {
                tcpManager = new TcpManager();
                HttpContext.Current.Application[AppKey] = tcpManager;
            }
            Thread t = new Thread(() =>
            {
                int port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                TcpListener listener = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    listener = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    listener.Start();

                    // Buffer for reading data
                    byte[] bytes = new byte[256];
                    string data = null;


                    // Enter the listening loop.
                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = listener.AcceptTcpClient();
                        Debug.WriteLine("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i = stream.Read(bytes, 0, bytes.Length);
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Debug.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper().Trim();
                        Debug.WriteLine(data);
                        int clientId = int.Parse(data);

                        tcpManager.AddClient(client, clientId);
                        Debug.WriteLine($"Added client {clientId}");
                    }

                }
                catch (SocketException e)
                {
                    Debug.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    listener.Stop();
                }
            });

            t.Start();
        }
    }
}