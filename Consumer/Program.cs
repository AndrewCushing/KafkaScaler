using Consumer.Config;
using Consumer.EventBus;
using Consumer.HostedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Consumer
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
                    services.AddHostedService<HostedConsumerService>();
                    services.Configure<KafkaConfig>(hostContext.Configuration.GetSection(nameof(KafkaConfig)));
                    services.AddSingleton<IEventBusConsumer, KafkaConsumer>();
                });
        }
    }
}