using System;
using System.Threading;
using System.Threading.Tasks;
using Consumer.Config;
using Consumer.EventBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consumer.HostedServices
{
    public class HostedConsumerService : BackgroundService
    {
        private readonly IEventBusConsumer _consumer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILogger<HostedConsumerService> _logger;

        public HostedConsumerService(ILogger<HostedConsumerService> logger, IOptions<KafkaConfig> kafkaConfig, IEventBusConsumer consumer)
        {
            _logger = logger;
            _kafkaConfig = kafkaConfig.Value;
            _consumer = consumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _consumer.ConsumeBatch(_kafkaConfig.Topic);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}