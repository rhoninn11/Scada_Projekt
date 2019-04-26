using System;
using System.Net;
using System.Net.Sockets;

namespace HubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry entryPoint = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = entryPoint.AddressList[0];
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, 5454);


            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            serverSocket.Bind(serverEndPoint);
            System.Console.WriteLine($"Sever is listning on: {serverEndPoint.Address}:{serverEndPoint.Port}");
            serverSocket.Listen(3);

            Socket clientSocket = serverSocket.Accept();
        }
    }
}
