using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using WorkerService.Config;

namespace WorkerService.EventBus
{
    public class KafkaProducer : IEventBusProducer
    {
        private readonly ILogger<Worker> _logger;
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer(IOptions<KafkaConfig> kafkaConfig, ILogger<Worker> logger)
        {
            _logger = logger;
            KafkaConfig kafkaConfig1 = kafkaConfig.Value;
            var producerConfig = new ProducerConfig
            {
                BootstrapServers = kafkaConfig1.BootstrapServers,
                Acks = Acks.All,
                ClientId = Dns.GetHostName(),
                LingerMs = 150,
                BatchNumMessages = 1000000,
                CompressionType = CompressionType.Snappy
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public void Produce(string eventBusName, string message)
        {
            _producer.Produce(eventBusName, new Message<Null, string> {Value = message}, report =>
            {
                if (report.Error.IsError)
                    _logger.LogError(
                        "Error while producing message [{message}] to topic [{topic}]. Error code: [{code}]. Error reason: [{reason}]",
                        message, eventBusName, report.Error.Code, report.Error.Reason);
                else
                    _logger.LogDebug("Produced message [{message}] to topic [{topic}]", message, eventBusName);
            });
        }

        public void Dispose()
        {
            _logger.LogInformation("Received request to dispose Kafka producer");
            _producer.Flush();
            _logger.LogDebug("Flushing of producer completed.");
            _producer.Dispose();
            _logger.LogDebug("Disposed of producer.");
        }
    }
}