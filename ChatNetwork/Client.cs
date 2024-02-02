using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ChatCommon;

namespace ChatNetwork
{
    public class Client<T>
    {
        private string _name;
        private IMessageSourceClient<T> _client;

        public Client(string n, IMessageSourceClient<T> cl)
        {
            this._name = n;
            this._client = cl;
        }

        private void ClientListener()
        {
            T remoteEndPoint = _client.CreateNewT();

            while (true)
            {
                try
                {
                    var messageReceived = _client.Receive(ref remoteEndPoint);

                    Console.WriteLine($"Получено сообщение от {messageReceived.FromName}:");
                    Console.WriteLine(messageReceived.Text);

                    Confirm(messageReceived, remoteEndPoint);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при получении сообщения: " + ex.Message);
                }
            }
        }

        private void Confirm(Message m, T remoteEndPoint)
        {
            var message = new Message() { FromName = _name, ToName = null, Text = null, Id = m.Id, Command = Command.Confirmation };
            _client.Send(message, remoteEndPoint);
        }


        private void Register(T remoteEndPoint)
        {
            var chatMessage = new Message() { FromName = _name, ToName = null, Text = null, Command = Command.Register };

            _client.Send(chatMessage, remoteEndPoint);
        }

        private void ClientSender()
        {

            Register(_client.GetServer());

            while (true)
            {
                try
                {
                    Console.WriteLine("NetMQ клиент ожидает ввода сообщения");

                    Console.Write("Введите  имя получателя и сообщение и нажмите Enter: ");
                    var messages = Console.ReadLine().Split(' ');

                    var message = new Message() { Command = Command.Message, FromName = _name, ToName = messages[0], Text = messages[1] };

                    _client.Send(message, _client.GetServer());
                    Console.WriteLine("Сообщение отправлено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }

        public void Start()
        {
            new Thread(() => ClientListener()).Start();

            ClientSender();
        }
    }



}
