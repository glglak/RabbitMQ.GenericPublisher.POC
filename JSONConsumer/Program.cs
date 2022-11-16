using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssetsConsumer
{
    internal class Program
    {
        private const string UName = "guest";

        private const string Pwd = "guest";

        private const string HName = "localhost";
        

        static void Main(string[] args)

        {


           
            string queueName = "Message.Content.*";


            ConnectionFactory factory = new ConnectionFactory

            {

                HostName = HName,

                UserName = UName,

                Password = Pwd,

            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {


                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }

            Console.WriteLine(" Press [enter] to exit.");


            Console.ReadLine();

        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
                   ? string.Join(" ", args)
                   : "info: Hello World!");
        }
    }
}