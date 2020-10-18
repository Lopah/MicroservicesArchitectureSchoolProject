using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Events.Orders
{
    public class CreateOrderEvent
    {
        public Guid UserId { get; set; }
        public List<Guid> ProductIds { get; set; } = new List<Guid>( );
    }
}
