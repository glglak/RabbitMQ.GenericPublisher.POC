using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.GenericPublisher.Models;
using System;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace RabbitMQ.Main;
public class Helper
{
    #region some variables
    private const string UName = "guest";

    private const string PWD = "guest";

    private const string HName = "localhost";

    private const string ExchangeTopic = "correlation.topic";

    private const string AssetsQueue = "Message.*.Assets";

    private const string JSONQueue = "Message.*.JSON";

    #endregion



    public static bool DropMessages(List<AssetTopic> messages)
    {
        ConnectionFactory factory = CreateRabbitMqConnection();

        using (IConnection conn = factory.CreateConnection())
        {
            if (conn.IsOpen == true)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messages[i]));
                        channel.BasicPublish(
                                                exchange: ExchangeTopic,
                                                routingKey: AssetsQueue,
                                                mandatory: true,
                                                basicProperties: null,
                                                body: message) ;
                    }
                }
            }
        }

        return true;
    }

    public static bool DropMessages(List<JsonTopic> messages)
    {
        ConnectionFactory factory = CreateRabbitMqConnection();

        using (IConnection conn = factory.CreateConnection())
        {
            if (conn.IsOpen == true)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    using (IModel channel = conn.CreateModel())
                    {
                        var message = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messages[i]));
                        channel.BasicPublish(
                                                exchange: ExchangeTopic,
                                                routingKey: JSONQueue,
                                                mandatory: true,
                                                basicProperties: null,
                                                body: message);
                    }
                }
            }
        }

        return true;
    }


     public static bool DropMessages(JsonObject messages,string topic,string queue)
    {
        ConnectionFactory factory = CreateRabbitMqConnection();

        using (IConnection conn = factory.CreateConnection())
        {
            if (conn.IsOpen == true)
            {
                 
                    using (IModel channel = conn.CreateModel())
                    {
                    var message = Encoding.UTF8.GetBytes(messages.ToString()); ;
                        channel.BasicPublish(
                                                exchange: topic,
                                                routingKey: queue,
                                                mandatory: true,
                                                basicProperties: null,
                                                body: message);
                    }
                
            }
        }

        return true;
    }

    private static ConnectionFactory CreateRabbitMqConnection()
    {
        ConnectionFactory factory = new ConnectionFactory();
        factory.UserName =UName;
        factory.Password = PWD;
        //factory.VirtualHost = cmdInstruction.VHostName;
        factory.HostName = HName; // hostname
      //  factory.Port = cmdInstruction.Port; // port;
      //  factory.RequestedHeartbeat = new TimeSpan(0, 0, 30);

        //if (cmdInstruction.Scheme.ToLowerInvariant().Trim() == "amqps")
        //{
        //    factory.Ssl.Enabled = true;
        //    factory.Ssl.Version = SslProtocols.Tls12;
        //    factory.Ssl.ServerName = cmdInstruction.HostName;

        //    // certificate for rabbit
        //    string certificateName = "certificate.cer";
        //    factory.Ssl.CertPath = CertificateHelper.GetCertificateFilePathName(certificateName);
        //}

        return factory;
    }
    

    //MD5 should be good for not colliding, hashing is better, but that could ensure readability and % of collide is slim 
    //could be better though..
    public  static async Task<Guid> GetReproducibleGuid(string businessId)
    {
         return  await  Task.Run(() =>
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


        }).ConfigureAwait(false);
       
    }
     
}