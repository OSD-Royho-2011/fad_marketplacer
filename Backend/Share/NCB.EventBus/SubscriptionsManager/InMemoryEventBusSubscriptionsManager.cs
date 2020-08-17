using NCB.EventBus.Abstractions;
using NCB.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCB.EventBus.SubscriptionsManager
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<Type>> _handlers;

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<Type>>();
        }

        public void Clear() => _handlers.Clear();

        public bool IsEmpty => _handlers.Keys.Any();

        public bool HasSubscriptionsForEvent(string routingKey) => _handlers.ContainsKey(routingKey);

        public IEnumerable<Type> GetHandlersForEvent(string routingKey) => _handlers[routingKey];

        public void AddSubscription<TH>(string routingKey) where TH : IEventHandler
        {
            var handlerType = typeof(TH);

            if (!HasSubscriptionsForEvent(routingKey))
            {
                _handlers.Add(routingKey, new List<Type>());
            }

            if (_handlers[routingKey].Any(s => s == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{routingKey}'");
            }

            _handlers[routingKey].Add(handlerType);
        }

        public void RemoveSubscription<TH>(string routingKey) where TH : IEventHandler
        {
            var handlerType = typeof(TH);

            if (HasSubscriptionsForEvent(routingKey) && _handlers[routingKey].Any(s => s == handlerType))
            {
                _handlers[routingKey].Remove(handlerType);

                if (!_handlers[routingKey].Any())
                {
                    _handlers.Remove(routingKey);
                }
            }
        }
    }
}
