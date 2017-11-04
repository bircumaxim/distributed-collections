using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Common;
using Common.Messages.DataRequest;
using Common.Messages.DataResponse;
using Common.Messages.DataResponse.Binary;
using Common.Messages.DataResponse.Json;
using Common.Messages.DataResponse.xml;
using Common.Models;
using Serialization.WireProtocol;
using Transport.Connectors.Tcp;
using Transport.Events;

namespace Client
{
    internal class Program
    {
        private static readonly string XmlValidationSchemaFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "EmployeeValidationSchema.xsd");

        private static readonly string JsonValidationSchemaFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "EmployeeValidationSchema.json");

        private static TcpConnector _tcpConnector;
        private const string MediatorIp = "127.0.0.1";
        private const int MediatorPort = 4000;

        public static void Main(string[] args)
        {
            _tcpConnector = new TcpConnector(GetSocket(), new DefaultWireProtocol());
            _tcpConnector.StateChanged += OnConnectorStateChanged;
            _tcpConnector.MessageReceived += OnMessageReceived;
            _tcpConnector.StartAsync();
            Console.ReadLine();
        }

        private static void OnConnectorStateChanged(object sender, ConnectorStateChangeEventArgs args)
        {
            if (args.NewState == ConnectionState.Connected)
            {
                var requestMessage = new DataRequestMessageBuilder()
                    .OrderBy(empl => empl.Age)
                    .WithTimeout(2000)
                    .DataType(DataType.Json)
                    .Build();
                _tcpConnector.SendMessage(requestMessage);
            }
        }

        private static void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(BinaryDataResponseMessage).Name)
            {
                var message = (BinaryDataResponseMessage) args.Message;
                Console.WriteLine(message.EmployeeMessages.Length);
                message.EmployeeMessages.ToList().ForEach(Console.WriteLine);
            }
            if (args.Message.MessageTypeName == typeof(XmlDataResponseMessage).Name)
            {
                var message = (XmlDataResponseMessage) args.Message;
                Console.WriteLine(message.EmployeeMessages);
                var emplyeeList = XmlHelper.Deserealize(message.EmployeeMessages);
                emplyeeList?.Employees?.ForEach(Console.WriteLine);
                XmlHelper.ValidateXml(message.EmployeeMessages, XmlValidationSchemaFilePath,
                    (s, e) => { Console.WriteLine(e.Message); });
            }
            if (args.Message.MessageTypeName == typeof(JsonDataResponseMessage).Name)
            {
                var message = (JsonDataResponseMessage) args.Message;
                Console.WriteLine(message.EmployeeMessages.Replace("\\", "").Replace("/", "").Replace(" ", ""));
                var emplyeeList = JsonHelper.Deserealize(message.EmployeeMessages);
                emplyeeList?.ForEach(Console.WriteLine);
                JsonHelper.ValidateJson(message.EmployeeMessages, JsonValidationSchemaFilePath,
                    (s, e) => Console.WriteLine(e.Message));
            }
        }

        private static Socket GetSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(MediatorIp), MediatorPort));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return socket;
        }
    }
}