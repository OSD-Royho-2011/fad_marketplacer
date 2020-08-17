using System;
using System.Collections.Generic;
using System.Text;

namespace NCB.EventBus.Models
{
    public class RabbitMQAppsettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public int RetryCount { get; set; }
        public string Vhost { get; set; }
        public int Port { get; set; }
        public string QueueName { get; set; }
        public string Exchange { get; set; }
    }
}
