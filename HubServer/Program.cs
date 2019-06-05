using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
                listner.Listen(10);

                Console.WriteLine($"Sever is listning on: {serverEndPoint.Address}:{serverEndPoint.Port}");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                listner.Close();
                return;
            }

            ManagingService mg = new ManagingService(listner);
            mg.WaitFornewSensors();



        }
    }

    class ManagingService
    {
        Socket listner;
        List<SensorHandler> sensors = new List<SensorHandler>();
        Task readingTask;

        public ManagingService(Socket listner)
        {
            this.listner = listner;
            this.readingTask = new Task(this.ReadAllSensorData);
            this.readingTask.Start();

        }

        public void WaitFornewSensors()
        {
            try
            {
                Console.WriteLine("Weatiting Connection");
                while (true)
                {
                    Socket clientlikeSensor = listner.Accept();

                    sensors.Add(new SensorHandler(clientlikeSensor));
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                listner.Close();
            }

        }

        public void ReadAllSensorData()
        {
            while (true)
            {
                if (sensors.Count == 0)
                    continue;

                System.Console.Write("");
                if (sensors.All(sh => sh.signalQueue.Count != 0))
                {
                    StringBuilder info = new StringBuilder();
                    int i = 1;
                    
                    sensors.ForEach(s =>
                    {   
                        while(s.signalQueue.Count > 1)
                            s.signalQueue.Dequeue();

                        int count = s.signalQueue.Count;
                        SignalData data = s.signalQueue.Dequeue();
                        info.Append($"sensor:{i++} wartość:{data.value}, timestamp:{data.timeSpan}, {count}\n");
                    });
                    System.Console.Clear();
                    System.Console.WriteLine(info.ToString());
                }
                
            }
        }
    }

    class SensorHandler
    {

        Socket sensorSocket;

        public Queue<SignalData> signalQueue;

        Task dataGatteringtask;
        public SensorHandler(Socket soc)
        {
            this.sensorSocket = soc;
            this.signalQueue = new Queue<SignalData>();
            this.dataGatteringtask = new Task(this.GetSignalData);
            this.dataGatteringtask.Start();
        }

        public void GetSignalData()
        {
            byte[] buffer = new Byte[16];
            while (true)
            {
                int bytesRecived = sensorSocket.Receive(buffer);
                if (bytesRecived == 16)
                {
                    SignalData data = new SignalData().FromBytes(buffer);
                    signalQueue.Enqueue(data);
                }
            }
        }
    }
}
