using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientlikeSensor
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress address = null;
            int portNUmber = 0;
            int sensorId = 0;

            if (args.Length < 3)
            {
                System.Console.WriteLine("Not enougth arguments:\n\r1: Ip addres\n\r2: Port number\n\r3: sensor id");
                return;
            }


            try
            {
                address = IPAddress.Parse(args[0]);
                portNUmber = Convert.ToInt32(args[1]);
                sensorId = Convert.ToInt32(args[2]);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

            IPHostEntry entryPoint = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = entryPoint.AddressList[0];
            IPEndPoint serverEndPoint = new IPEndPoint(address, portNUmber);


            Socket sensor = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sensor.Connect(serverEndPoint);
                string message = $"Hello form sensor {sensorId}\r";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                int bytesSend = sensor.Send(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
