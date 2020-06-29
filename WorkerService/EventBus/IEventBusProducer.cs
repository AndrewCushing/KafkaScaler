using System.Threading.Tasks;

namespace WorkerService.EventBus
{
    public interface IEventBusProducer
    {
        public void Produce(string eventBusName, string message);

        public void Dispose();
    }
}