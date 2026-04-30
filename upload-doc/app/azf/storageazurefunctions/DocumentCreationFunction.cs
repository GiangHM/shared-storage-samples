using System.Text;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sharedentities.DBEntities;

namespace storageazurefunctions
{
    public class DocumentCreationFunction
    {
        private readonly ILogger<DocumentCreationFunction> _logger;

        public DocumentCreationFunction(ILogger<DocumentCreationFunction> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This function is triggered when a message is received from a Service Bus topic subscription.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageActions"></param>
        /// <returns></returns>
        [Function(nameof(DocumentCreationFunction))]
        [SqlOutput("dbo.storagedocument", connectionStringSetting: "SqlConnectionString")]
        public async Task<StorageDocument> Run(
            [ServiceBusTrigger("mytopic", "mysubscription", Connection = "", AutoCompleteMessages = false)]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            //TODO: Deep dive into the message properties and body
            // Deep dive into the CompleteMessageAsync & prop: AutoCompleteMessages
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            var document = JsonConvert.DeserializeObject<StorageDocument>
                (Encoding.UTF8.GetString(message.Body));

            await messageActions.CompleteMessageAsync(message);
            return document;
        }
    }
}
