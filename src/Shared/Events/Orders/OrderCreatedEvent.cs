using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Events.Orders
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }

        public List<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
