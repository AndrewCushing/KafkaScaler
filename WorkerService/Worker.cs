using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkerService.Config;
using WorkerService.EventBus;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEventBusProducer _eventBusProducer;

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