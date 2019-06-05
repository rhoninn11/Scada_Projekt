using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

using ClientlikeSensor;

namespace HubServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress address;
            int portNumber;
            if (ChcekArgs(args, out address, out portNumber) < 0)
                return;

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

            ManagingService mg = new ManagingService(listner, address, portNumber);
            mg.WaitFornewSensors();



        }

        public static int ChcekArgs(string[] args, out IPAddress address, out int portNUmber)
        {
            address = null;
            portNUmber = 0;


            if (args.Length < 2)
            {
                System.Console.WriteLine("Not enougth arguments:\n\r1: rasp ip\n\r2: Port number\n\r");
                return -1;
            }

            try
            {
                address = IPAddress.Parse(args[0]);
                portNUmber = Convert.ToInt32(args[1]);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return -1;
            }
            return 1;
        }
    }

    class ManagingService
    {
        Socket listner;
        List<SensorHandler> sensors = new List<SensorHandler>();
        Task readingTask;
        Task dataSendingTask;
        bool isFramReady = false;
        JsonData SensorDataBuffer;

        Socket rasp;

        public ManagingService(Socket listner, IPAddress raspIp, int port)
        {
            this.SensorDataBuffer = new JsonData();
            this.listner = listner;
            this.ConnectToRasp(raspIp, port);
            this.readingTask = new Task(this.ReadAllSensorData);
            this.readingTask.Start();
            this.dataSendingTask = new Task(this.sendingData);
            this.dataSendingTask.Start();

        }

        public void WaitFornewSensors()
        {
            if (this.rasp == null)
                return;
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
            if (this.rasp == null)
                return;

            while (true)
            {
                if (sensors.Count == 0)
                    continue;

                System.Console.Write("");
                if (sensors.All(sh => sh.signalQueue.Count != 0))
                {
                    StringBuilder info = new StringBuilder();
                    int i = 1;
                    List<double> sensorValues = new List<double>();
                    sensors.ForEach(s =>
                    {
                        while (s.signalQueue.Count > 1)
                            s.signalQueue.Dequeue();

                        int count = s.signalQueue.Count;
                        SignalData data = s.signalQueue.Dequeue();
                        sensorValues.Add(data.value);
                        info.Append($"sensor:{i++} wartość:{data.value}, timestamp:{data.timeSpan}, {count}\n");
                    });

                    this.SensorDataBuffer.SensorData = sensorValues;
                    double sum = 0;
                    int c = sensorValues.Count;
                    sensorValues.ForEach(v => sum += v);
                    this.SensorDataBuffer.ProcessedData = sum/c;
                    this.isFramReady = true;

                    System.Console.Clear();
                    System.Console.WriteLine(info.ToString());
                }

            }
        }

        public void sendingData()
        {
            if (this.rasp == null)
                return;

            while (true)
            {

                if(!this.isFramReady)
                    continue;

                byte[] message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this.SensorDataBuffer));
                this.isFramReady = false;
                this.rasp.Send(message);
            }

        }

        public void ConnectToRasp(IPAddress address, int portNumber)
        {
            IPHostEntry entryPoint = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = entryPoint.AddressList[0];
            IPEndPoint serverEndPoint = new IPEndPoint(address, portNumber);


            Socket nodeSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                nodeSocket.Connect(serverEndPoint);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return;
            }
            this.rasp = nodeSocket;
        }
    }

    class JsonData
    {
        public double ProcessedData { get; set; }
        public List<double> SensorData { get; set; }

        override public string ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append($"procesed Value{this.ProcessedData}\n [");
            this.SensorData.ForEach(d => sb.Append($"{d}"));
            sb.Append("]\n");
            return sb.ToString();
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
