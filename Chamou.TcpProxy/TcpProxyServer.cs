using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chamou.TcpProxy
{
    public class TcpProxyServer
    {
        private Thread _deviceThread;
        private Thread _serviceThread;

        public void Start()
        {
            ClientManager tcpManager = ClientManager.Instance;

            int devicePort = 13000, servicePort = 13001;
            IPAddress localAddr = IPAddress.Any;
            
            _deviceThread = new Thread(() =>
            {
                // TcpListener server = new TcpListener(port);
                TcpListener listener = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    listener = new TcpListener(localAddr, devicePort);

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
                        Console.WriteLine("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i = stream.Read(bytes, 0, bytes.Length);
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper().Trim();
                        Console.WriteLine(data);
                        try
                        {
                            int clientId = int.Parse(data);
                            tcpManager.AddClient(client, clientId);
                            Console.WriteLine($"Added client {clientId}");
                        }
                        catch { Console.WriteLine($"Invalid message: {data}"); }
                        
                    }

                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    listener.Stop();
                }
            });
            
            _serviceThread = new Thread(() =>
            {
                // TcpListener server = new TcpListener(port);
                TcpListener listener = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    listener = new TcpListener(localAddr, servicePort);

                    // Start listening for client requests.
                    listener.Start();

                    // Buffer for reading data
                    byte[] bytes = new byte[256];
                    string data = null;


                    // Enter the listening loop.
                    while (true)
                    {
                        Console.Write("Waiting for a connection on Service end... ");

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Service Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i = stream.Read(bytes, 0, bytes.Length);
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received from Service: {0}", data);
                        // Process the data sent by the client.
                        data = data.Trim();
                        try
                        {
                            var parts = data.Split(new[] { ';' });
                            int clientId = int.Parse(parts[0]);
                            string message = parts[1];
                            tcpManager.SendMessage(clientId, message);
                            Console.WriteLine($"Sent message {message} to client {clientId}");
                            
                        }
                        catch { Console.WriteLine($"Invalid message: {data}"); }

                        client.Close();
                    }

                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    listener.Stop();
                }
            });

            _deviceThread.Start();
            _serviceThread.Start();
        }

        public void Join() { _deviceThread.Join(); _serviceThread.Join(); }

    }
}
