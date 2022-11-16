using RabbitMQ.Client;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RabbitMQ.Main;
public class Publisher
{
    private const string UName = "guest";

    private const string PWD = "guest";

    private const string HName = "localhost";



    public void SendMessage()

    {

        //Main entry point to the RabbitMQ .NET AMQP client

        var connectionFactory = new ConnectionFactory()

        {

            UserName = UName,

            Password = PWD,

            HostName = HName

        };

        var connection = connectionFactory.CreateConnection();

        var model = connection.CreateModel();

        var properties = model.CreateBasicProperties();

        properties.Persistent = false;

        byte[] messagebuffer = Encoding.Default.GetBytes("Message from Topic Exchange 'Bombay' ");

        model.BasicPublish("topic.exchange", "Message.Bombay.Email", properties, messagebuffer);

        Console.WriteLine("Message Sent From: topic.exchange ");

        Console.WriteLine("Routing Key: Message.Bombay.Email");

        Console.WriteLine("Message Sent");

    }

    //MD5 should be good for not colliding, hashing is better, but that could ensure readability and % of collide is slim 
    //could be better though..
    public  Guid GetReproducibleGuid(string businessId)
    {
        if (!string.IsNullOrEmpty(businessId))
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(businessId));
                Guid result = new Guid(hash);
                return result;
            }
        }
        throw new Exception("empty string");

    }
     
}