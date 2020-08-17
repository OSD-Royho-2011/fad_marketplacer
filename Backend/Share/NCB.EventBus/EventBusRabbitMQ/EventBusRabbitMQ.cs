using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NCB.EventBus.Abstractions;
using NCB.EventBus.Models;
using NCB.EventBus.SubscriptionsManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace NCB.EventBus.EventBusRabbitMQ
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;

        private readonly string _exchange;
        private readonly string _queueName;
        private readonly int _retryCount;
        private IModel _consumerChannel;

        public EventBusRabbitMQ(
            IRabbitMQPersistentConnection persistentConnection,
            ILogger<EventBusRabbitMQ> logger,
            IEventBusSubscriptionsManager subsManager,
            IServiceProvider serviceProvider,
            IOptions<RabbitMQAppsettings> rabbitMQ
            )
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _serviceProvider = serviceProvider;
            _queueName = rabbitMQ.Value.QueueName;
            _exchange = rabbitMQ.Value.Exchange;
            _retryCount = rabbitMQ.Value.RetryCount;
            _consumerChannel = CreateConsumerChannel();
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }

        public void Publish(string routingKey, dynamic data)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, $"Could not publish event: {routingKey} after {time.TotalSeconds}s ({ex.Message})");
                });


            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: _exchange, type: "direct");

                var message = JsonConvert.SerializeObject(data);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    channel.BasicPublish(
                        exchange: _exchange,
                        routingKey: routingKey,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }
        }

        public void Subscribe<TH>(string routingKey) where TH : IEventHandler
        {
            //DoInternalSubscription(routingKey);
            if (!_subsManager.HasSubscriptionsForEvent(routingKey))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: _exchange,
                                      routingKey: routingKey);
                }
            }

            _subsManager.AddSubscription<TH>(routingKey);
            StartBasicConsume();
        }

        public void UnSubscribe<TH>(string routingKey) where TH : IEventHandler
        {
            _subsManager.RemoveSubscription<TH>(routingKey);

            if (!_subsManager.HasSubscriptionsForEvent(routingKey))
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueUnbind(queue: _queueName,
                        exchange: _exchange,
                        routingKey: routingKey);

                    if (_subsManager.IsEmpty)
                    {
                        _consumerChannel.Close();
                    }
                }
            }
        }

        //private

        //private void DoInternalSubscription(string routingKey)
        //{
        //    var containsKey = _subsManager.HasSubscriptionsForEvent(routingKey);
        //    if (!containsKey)
        //    {
        //        if (!_persistentConnection.IsConnected)
        //        {
        //            _persistentConnection.TryConnect();
        //        }

        //        using (var channel = _persistentConnection.CreateModel())
        //        {
        //            channel.QueueBind(queue: _queueName,
        //                              exchange: _exchange,
        //                              routingKey: routingKey);
        //        }
        //    }
        //}

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private void StartBasicConsume()
        {
            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                //subcribe event
                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var routingKey = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            try
            {
                await ProcessEvent(routingKey, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"----- ERROR Processing message: \"{message}\"");
            }

            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string routingKey, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(routingKey))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var eventHandlers = _subsManager.GetHandlersForEvent(routingKey);
                    foreach (var eventHandler in eventHandlers)
                    {
                        var handler = scope.ServiceProvider.GetService(eventHandler) as IEventHandler;
                        if (handler == null) continue;
                        dynamic eventData = JObject.Parse(message);

                        await Task.Yield();// what is this?
                        await handler.Handle(eventData);
                    }
                }
            }
            else
            {
                _logger.LogError($"No subscription for RabbitMQ event: {routingKey}");
            }
        }
    }
}
