using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using ClientlikeSensor;

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

                    byte[] buffer = new Byte[16];
                    String sBuffer = String.Empty;

                    while (true)
                    {
                        int bytesRecived = clientlikeSensor.Receive(buffer);
                        if(bytesRecived == 16){
                            SignalData data = new SignalData().FromBytes(buffer);
                            Console.Clear();
                            Console.WriteLine($"Recived data - value:{data.value},\n\ttime stamp:{data.timeSpan}");

                        }
                    }

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
