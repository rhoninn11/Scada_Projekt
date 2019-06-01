using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry entryPoint = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = entryPoint.AddressList[0];
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, 5454);


            Socket listner = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listner.Bind(serverEndPoint);
                listner.Listen(3);

                Console.WriteLine($"Sever is listning on: {serverEndPoint.Address}:{serverEndPoint.Port}");

                while (true)
                {
                    Console.WriteLine("Weatiting Connection");
                    Socket clientlikeSensor = listner.Accept();

                    byte[] buffer = new Byte[1024];
                    String sBuffer = String.Empty;

                    while (true)
                    {
                        int bytesRecived = clientlikeSensor.Receive(buffer);
                        sBuffer += Encoding.UTF8.GetString(buffer, 0, bytesRecived);
                        if(sBuffer.Contains('\r'))
                            break;
                    }

                    Console.WriteLine($"Recived data: {sBuffer}");

                    byte[] messageToSensor = Encoding.UTF8.GetBytes("Jest komunikcaja");

                    clientlikeSensor.Send(messageToSensor);

                    clientlikeSensor.Shutdown(SocketShutdown.Both);
                    clientlikeSensor.Close();
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            listner.Close();
        }
    }
}
