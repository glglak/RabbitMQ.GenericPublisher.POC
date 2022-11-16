using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.GenericPublisher.Models;
using RabbitMQ.Main;
using System.Text.Json.Nodes;

namespace RabbitMQ.GenericPublisher.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GenericPublisherController : ControllerBase
    {


        private readonly ILogger<GenericPublisherController> _logger;

        public GenericPublisherController(ILogger<GenericPublisherController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="genericJson"></param>
        /// <param name="topic"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        [HttpPost(Name = "PublishGeneric")]
        public async Task<IActionResult> PublishGeneric([FromBody] object genericJson, string topic, string queue)
        {

            _logger.LogTrace("Recieved:" + JsonConvert.SerializeObject(genericJson) + " at " + DateTime.UtcNow);
            //for testing purposes


            Helper.DropMessages(genericJson, topic, queue);
            _logger.LogTrace("Finished at " + DateTime.UtcNow);
            return Ok("done and gone");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetTopic"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost(Name = "PublishJson")]
     
        public async Task<IActionResult> PublishJson([FromBody] List<JsonTopic> jsonsDocs)
        {

            _logger.LogTrace("Recieved:" + JsonConvert.SerializeObject(jsonsDocs) + " at " + DateTime.UtcNow);
            //for testing purposes

            for (int i = 0; i < 50000; i++)
            {
                JsonTopic jsonTopic = new JsonTopic();
                jsonTopic.BusinessId = "message number:" + i.ToString();
                jsonTopic.CorrelationId = await Helper.GetReproducibleGuid(jsonTopic.BusinessId);
                jsonTopic.CreatedAt = DateTime.UtcNow;
               
                Console.WriteLine(i);
                jsonsDocs.Add(jsonTopic);
            }

            Helper.DropMessages(jsonsDocs);
            _logger.LogTrace("Finished at " + DateTime.UtcNow);
            return Ok("done and gone");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assets"></param>
        /// <returns></returns>
        [HttpPost(Name = "PublishAsset")]
         
        public async Task<IActionResult> PublishAsset([FromBody] List<AssetTopic> assets)
        {
            _logger.LogTrace("Recieved:" + JsonConvert.SerializeObject(assets) + " at " + DateTime.UtcNow);
            //for testing purposes
           
            for (int i=0;i<50000;i++)
            {
                 AssetTopic assetTopic = new AssetTopic();
                assetTopic.BusinessId = "message number:" + i.ToString();
                assetTopic.CorrelationId = await Helper.GetReproducibleGuid(assetTopic.BusinessId);
                assetTopic.CreatedAt = DateTime.UtcNow;
                assetTopic.VitriumId = i.ToString();
               
                Console.WriteLine(i);
                assets.Add(assetTopic);
            }

            Helper.DropMessages(assets);
            _logger.LogTrace("Finished at " + DateTime.UtcNow);
            return Ok("done and gone");
        }
      

    }
}