using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ClientlikeSensor
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress address;
            int portNumber, sensorId, processId;
            if (ChcekArgs(args, out address, out portNumber, out sensorId, out processId) < 0)
                return;

            IPHostEntry entryPoint = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = entryPoint.AddressList[0];
            IPEndPoint serverEndPoint = new IPEndPoint(address, portNumber);


            Socket sensorSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Sensor sensor = new Sensor(sensorId, processId);
            try
            {
                sensorSocket.Connect(serverEndPoint);
                while (true)
                {
                    if (sensor.dataQueue.Count == 0)
                        continue;

                    SignalData data = sensor.dataQueue.Dequeue();
                    byte[] frame = data.ToBytes();

                    sensorSocket.Send(frame);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int ChcekArgs(string[] args, out IPAddress address, out int portNUmber, out int sensorId, out int processId)
        {
            address = null;
            portNUmber = 0;
            sensorId = 0;
            processId = 0;

            if (args.Length < 4)
            {
                System.Console.WriteLine("Not enougth arguments:\n\r1: Ip addres\n\r2: Port number\n\r3: sensor id\n\r4: messured process id");
                return -1;
            }

            try
            {
                address = IPAddress.Parse(args[0]);
                portNUmber = Convert.ToInt32(args[1]);
                sensorId = Convert.ToInt32(args[2]);
                processId = Convert.ToInt32(args[3]);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return -1;
            }
            return 1;
        }
    }

    class Sensor
    {
        private bool powerOffFlag = false;
        static float frequency = 0.12f;
        public int id;
        public int processId;
        private Task signalGeneration;
        public Queue<SignalData> dataQueue;
        public Sensor(int id, int processId)
        {
            this.id = id;
            this.processId = processId;
            this.dataQueue = new Queue<SignalData>();
            this.signalGeneration = new Task(this.GenerateSignal);
            this.signalGeneration.Start();
        }

        private void GenerateSignal()
        {
            while (!this.powerOffFlag)
            {

                SignalData data = new SignalData();
                data.timeSpan = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                data.value = Math.Sin(data.timeSpan / 1000.0 * 2 * Math.PI * frequency);
                dataQueue.Enqueue(data);

                System.Console.Clear();
                System.Console.WriteLine($"Pomiar czujnika id:{this.id}\n-dla procesu {this.processId} wynosi: {data.value}");
                Thread.Sleep(10);
            }
        }
    }

    public struct SignalData
    {
        public double value;
        public long timeSpan;

        public byte[] ToBytes()
        {
            byte[] valueAsBytes = BitConverter.GetBytes(this.value);
            byte[] timeSpanAsBytes = BitConverter.GetBytes(this.timeSpan);
            return valueAsBytes.Concat(timeSpanAsBytes).ToArray();
        }
        public SignalData FromBytes(byte[] bytes)
        {
            if (bytes.Length != 16)
            {
                System.Console.WriteLine("abd array length");
                return new SignalData();
            }

            this.value = BitConverter.ToDouble(bytes, 0);
            this.timeSpan = BitConverter.ToInt64(bytes, 8);

            return this;
        }
    }
}
