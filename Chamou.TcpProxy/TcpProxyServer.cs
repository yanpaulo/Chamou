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
        private ClientManager _tcpManager = ClientManager.Instance;
        private static readonly int 
            _devicePort = 8080, _servicePort = 8081;
        private IPAddress _localAddr = IPAddress.Any;

        public void Run()
        {
            var deviceTask = AcceptClientDevicesAsync();

            var serviceTask = AcceptClientServicesAsync();

            Task.WaitAll(deviceTask, serviceTask);
        }

        #region Devices Task
        private async Task AcceptClientDevicesAsync()
        {
            TcpListener listener = new TcpListener(_localAddr, _devicePort);
            try
            {
                // Start listening for client requests.
                listener.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection on Device end... ");

                    var client = await listener.AcceptTcpClientAsync();
                    HandleClientDeviceAsync(client);
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
        }

        private async Task HandleClientDeviceAsync(TcpClient client)
        {
            //Buffer for reading data
            byte[] bytes = new byte[256];
            string data = null;

            Console.WriteLine("Device Connected!");

            try
            {
                NetworkStream stream = client.GetStream();

                int i = await stream.ReadAsync(bytes, 0, bytes.Length);
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                data = data.ToUpper().Trim();
                try
                {
                    int clientId = int.Parse(data);
                    _tcpManager.AddClient(client, clientId);
                    Console.WriteLine($"Added client {clientId}");
                }
                catch { Console.WriteLine($"Invalid message: {data}"); }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        } 
        #endregion

        #region Service Tasks

        private async Task AcceptClientServicesAsync()
        {
            // TcpListener server = new TcpListener(port);
            TcpListener listener = new TcpListener(_localAddr, _servicePort);
            try
            {
                listener.Start();

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection on Service end... ");

                    var client = await listener.AcceptTcpClientAsync();
                    HandleClientServiceAsync(client);

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
        }

        private async Task HandleClientServiceAsync(TcpClient client)
        {
            {
                // Buffer for reading data
                byte[] bytes = new byte[256];
                string data = null;
                int i;

                Console.WriteLine("Service Connected!");
                try
                {
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    while ((i = await stream.ReadAsync(bytes, 0, bytes.Length)) > 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received from Service: {0}", data);
                        // Process the data sent by the client Service.
                        data = data.Trim();
                        try
                        {
                            var parts = data.Split(new[] { ';' });
                            int clientId = int.Parse(parts[0]);
                            string message = parts[1];
                            string responseMessage;
                            if (_tcpManager.SendMessage(clientId, message))
                            {
                                responseMessage = $"Sent message '{message}' to client {clientId}\r\n";
                                Console.WriteLine(responseMessage);
                                await stream.WriteAsync(Encoding.ASCII.GetBytes(responseMessage), 0, responseMessage.Length);
                            }
                            else
                            {
                                responseMessage = $"Error: Client {clientId} not found\r\n";
                                Console.WriteLine(responseMessage);
                                await stream.WriteAsync(Encoding.ASCII.GetBytes(responseMessage), 0, responseMessage.Length);
                            }
                            
                        }
                        catch { Console.WriteLine($"Invalid message: {data}"); }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }

            }
        } 
        #endregion
    }
}
