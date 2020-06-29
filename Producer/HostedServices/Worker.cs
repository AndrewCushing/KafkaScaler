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
    public class Worker : BackgroundService
    {
        private readonly IEventBusProducer _eventBusProducer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILogger<Worker> _logger;

        public Worker(IOptions<KafkaConfig> kafkaConfig, ILogger<Worker> logger, IEventBusProducer eventBusProducer)
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
                _eventBusProducer.Produce(_kafkaConfig.Topic, $"Here is a message I made at {DateTime.UtcNow}");
                _logger.LogDebug("Worker produced a message.");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}