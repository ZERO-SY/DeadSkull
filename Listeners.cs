using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace DeadSkull_Lib
{
    public class DeadSkull_listener
    {
        private TcpListener listener;

        public void Start()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 500;
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine("Server is listening on " + listener.LocalEndpoint);

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                Socket client = listener.AcceptSocket();

                Thread childSocketThread = new Thread(() =>
                {
                    Console.WriteLine("Connection accepted.");

                    byte[] data = new byte[100];
                    int size = client.Receive(data);

                    Console.WriteLine("Received data: ");
                    for (int i = 0; i < size; i++)
                    {
                        Console.Write(Convert.ToChar(data[i]));
                    }

                    Console.WriteLine();

                    // Process the received data as a command and send a response
                    string command = Encoding.UTF8.GetString(data, 0, size);
                    string response = ProcessCommand(command);
                    byte[] responseData = Encoding.UTF8.GetBytes(response);
                    client.Send(responseData);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }

        private string ProcessCommand(string command)
        {
            // Process the command and generate a response
            // This is just a placeholder - replace with your own command processing logic
            return "Processed command: " + command;
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}