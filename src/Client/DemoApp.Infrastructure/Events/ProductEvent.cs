using System;

namespace DemoApp.Infrastructure.Events
{
    public class ProductEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
