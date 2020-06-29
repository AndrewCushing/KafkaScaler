using Microsoft.Extensions.Options;

namespace WorkerService.Config
{
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; }
    }
}