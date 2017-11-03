using System;
using Common;
using Serialization;
using Serialization.WireProtocol;
using Transport.Connectors.Tcp;
using Transport.Connectors.Tcp.Events;
using Transport.Events;

namespace Mediator
{
    public class ClientConnecionService
    {
        private TcpConnector _clienTcpConnector;
        public event MessageReceivedHandler MessageReceived;
        
        public ClientConnecionService(int port)
        {
            var tcpConnectionListener = new TcpConnectionListener(port);
            tcpConnectionListener.TcpClientConnected += OnClientConnected;
            tcpConnectionListener.StartAsync();
        }

        public void SendDataToClient(Message message)
        {
            _clienTcpConnector.SendMessage(message);
        }
        
        private void OnClientConnected(object sender, TcpClientConnectedEventArgs args)
        {
            _clienTcpConnector = new TcpConnector(args.ClientSocket, new DefaultWireProtocol());
            _clienTcpConnector.MessageReceived += OnMessageReceived;
            _clienTcpConnector.StartAsync();
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            MessageReceived?.Invoke(this, args);
        }
    }
}