using System.Collections.Generic;
using System.Net;
using Confluent.Kafka;
using Consumer.Config;
using Consumer.HostedServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Consumer.EventBus
{
    public class KafkaConsumer : IEventBusConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly KafkaConfig _kafkaConfig;
        private readonly ILogger<HostedConsumerService> _logger;

        public KafkaConsumer(IOptions<KafkaConfig> kafkaConfig, ILogger<HostedConsumerService> logger)
        {
            _kafkaConfig = kafkaConfig.Value;
            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                Acks = Acks.All,
                ClientId = Dns.GetHostName(),
                CheckCrcs = true,
                EnableAutoCommit = false,
                GroupId = "DemoConsumerGroup",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                FetchMinBytes = 512,
                FetchWaitMaxMs = 150
            };
            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        public List<string> ConsumeBatch(string eventBusTopic)
        {
            var messages = new List<string>();
            _consumer.Subscribe(eventBusTopic);
            var a = _consumer.Consume();
            while (a.Message != null)
            {
                messages.Add(a.Message.Value);
                _logger.LogInformation(a.Message.Value);
                a = _consumer.Consume();
            }

            _logger.LogInformation("Finished consuming messages.");
            return messages;
        }
    }
}