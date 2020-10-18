using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Events.Orders
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
        public decimal TotalPrice { get; set; }
        public class ProductDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Amount { get; set; }
        }
    }

        
}
