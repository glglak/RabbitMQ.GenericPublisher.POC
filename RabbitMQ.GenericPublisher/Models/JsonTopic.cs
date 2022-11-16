using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQ.GenericPublisher.Models
{
    [Serializable]

    public class JsonTopic
    {
        [NotMapped]
        public Guid CorrelationId { get; set; }
        [NotMapped]
        public DateTime CreatedAt { get; set; }

        public string BusinessId { get; set; }

        public string ProviderId { get; set; }

        public string ContentType { get; set; }
    }
}