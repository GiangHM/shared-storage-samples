using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace storageapi.Services
{
    public interface IAzureServiceBusHelper
    {
        Task SendMessageAsync<T>(T message, string queueOrTopicName);
    }
    public class AzureServiceBusHelper : IAzureServiceBusHelper
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<AzureServiceBusHelper> _logger;

        public AzureServiceBusHelper(ServiceBusClient serviceBusClient, ILogger<AzureServiceBusHelper> logger)
        {
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        public async Task SendMessageAsync<T>(T message, string queueOrTopicName)
        {
            try
            {
                var sender = _serviceBusClient.CreateSender(queueOrTopicName);
                var messageBody = JsonSerializer.Serialize(message);
                var serviceBusMessage = new ServiceBusMessage(messageBody);
                _logger.LogInformation("Sending message to {QueueOrTopic}: {MessageBody}", queueOrTopicName, messageBody);
                await sender.SendMessageAsync(serviceBusMessage);
                _logger.LogInformation("Message sent successfully to {QueueOrTopic}", queueOrTopicName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to {QueueOrTopic}", queueOrTopicName);
                throw;
            }
        }

        
    }


}
