using NCB.EventBus.Abstractions;
using NCB.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.EventBus.SubscriptionsManager
{
    public interface IEventBusSubscriptionsManager
    {
        void AddSubscription<TH>(string routingKey) where TH : IEventHandler;

        void RemoveSubscription<TH>(string routingKey) where TH : IEventHandler;

        bool HasSubscriptionsForEvent(string routingKey);

        IEnumerable<Type> GetHandlersForEvent(string routingKey);

        void Clear();

        bool IsEmpty { get; }
    }
}
