using ChatNetwork;
using System.Net;

namespace ChatApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var s = new Server<IPEndPoint>(new NetMQMessageSource());
                s.Work();
            }
            else
            if (args.Length == 3)
            {
                var c = new Client<IPEndPoint>(args[0], new NetMQMessageSourceClient(int.Parse(args[2]), args[1], 5555));
                c.Start();
            }
            else
            {

                Console.WriteLine("Введите ник для запуска сервера");
                Console.WriteLine("Введите Ник и IP сервера для запуска приложения");
            }
        }

    }
}