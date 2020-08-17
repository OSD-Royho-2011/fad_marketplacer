using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NCB.EventBus.Abstractions
{
    public interface IEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
