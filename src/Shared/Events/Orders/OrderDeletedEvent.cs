using System;
using System.Collections.Generic;

namespace DemoApp.Shared.Events.Orders
{
    public class OrderDeletedEvent
    {
        public Guid Id { get; set; }
        public ICollection<ProductDto> Products { get; set; }

        public class ProductDto
        {
            public Guid Id { get; set; }
            public int Amount { get; set; }
        }
    }
}
