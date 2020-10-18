using System;
using System.Collections.Generic;
using DemoApp.Infrastructure.SqlServer.DbEntities;

namespace DemoApp.Infrastructure.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }

        public ICollection<Product> Products { get; set; }

        public int ProductCount => Products.Count;
    }
}
