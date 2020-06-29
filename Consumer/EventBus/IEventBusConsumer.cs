using System.Collections.Generic;

namespace Consumer.EventBus
{
    public interface IEventBusConsumer
    {
        public List<string> ConsumeBatch(string eventBusTopic);
    }
}