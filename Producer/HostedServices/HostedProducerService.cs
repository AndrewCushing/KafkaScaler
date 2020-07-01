using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Producer.Config;
using Producer.EventBus;

namespace Producer.HostedServices
{
    public class HostedProducerService : BackgroundService
    {
        private readonly IEventBusProducer _eventBusProducer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILogger<HostedProducerService> _logger;

        public HostedProducerService(IOptions<KafkaConfig> kafkaConfig, ILogger<HostedProducerService> logger, IEventBusProducer eventBusProducer)
        {
            _kafkaConfig = kafkaConfig.Value;
            _logger = logger;
            _eventBusProducer = eventBusProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Worker about to produce a message.");
                _eventBusProducer.Produce(_kafkaConfig.Topic, $"This is a message I made at {DateTime.UtcNow}");
                _logger.LogDebug("Worker produced a message.");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}