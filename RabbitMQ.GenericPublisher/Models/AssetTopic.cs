using RabbitMQ.GenericPublisher.Controllers;
using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQ.GenericPublisher.Models
{
    [Serializable]

    public class AssetTopic
    {
        [NotMapped]
        public Guid CorrelationId { get; set; }
        [NotMapped]
        public DateTime CreatedAt { get; set; }

        public string BusinessId { get; set; }

        public string VitriumUrl { get; set; }

        public string VitriumId { get; set; }
    }
}