using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkerService.EventBus;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IEventBusProducer _eventBusProducer;
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IEventBusProducer eventBusProducer)
        {
            _logger = logger;
            _eventBusProducer = eventBusProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Worker about to produce a message.");
                _eventBusProducer.Produce("DummyTopic", $"Here is a message I made at {DateTime.UtcNow}");
                _logger.LogDebug("Worker produced a message.");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}