using ChatCommon;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatNetwork
{
    public class NetMQMessageSource : IMessageSource<IPEndPoint>
    {
        private readonly DealerSocket _dealerSocket;

        public NetMQMessageSource()
        {
            _dealerSocket = new DealerSocket();
            _dealerSocket.Bind("tcp://127.0.0.1:5555");
        }

        public IPEndPoint CreateNewT()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public IPEndPoint CopyT(IPEndPoint t)
        {
            return new IPEndPoint(t.Address, t.Port);
        }

        public Message Receive(ref IPEndPoint ep)
        {
            var message = _dealerSocket.ReceiveFrameString();
            return Message.FromJson(message);
        }

        public void Send(Message message, IPEndPoint ep)
        {
            _dealerSocket.SendFrame(message.ToJson());
        }
    }

    public class NetMQMessageSourceClient : IMessageSourceClient<IPEndPoint>
    {
        private readonly RequestSocket _requestSocket;

        public NetMQMessageSourceClient(int port, string ipAddress, int remotePort)
        {
            _requestSocket = new RequestSocket();
            _requestSocket.Connect($"tcp://{ipAddress}:{port}");
        }

        public IPEndPoint CreateNewT()
        {
            return new IPEndPoint(IPAddress.Any, 0);
        }

        public IPEndPoint GetServer()
        {
            return _requestSocket.Options.LastEndpoint;
        }

        public Message Receive(ref IPEndPoint iPEndPoint)
        {
            var message = _requestSocket.ReceiveFrameString();
            return Message.FromJson(message);
        }

        public void Send(Message message, IPEndPoint iPEndPoint)
        {
            _requestSocket.SendFrame(message.ToJson());
        }
    }
}
