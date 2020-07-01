using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Producer.Config;
using Producer.EventBus;
using Producer.HostedServices;

namespace Producer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HostedProducerService>();
                    services.Configure<KafkaConfig>(hostContext.Configuration.GetSection(nameof(KafkaConfig)));
                    services.AddSingleton<IEventBusProducer, KafkaProducer>();
                });
        }
    }
}