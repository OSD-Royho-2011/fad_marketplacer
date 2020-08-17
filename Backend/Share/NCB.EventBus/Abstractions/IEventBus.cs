using NCB.EventBus.Events;
using System;

namespace NCB.EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(string routingKey, dynamic data);

        void Subscribe<TH>(string routingKey) where TH : IEventHandler;

        void UnSubscribe<TH>(string routingKey) where TH : IEventHandler;
    }
}
