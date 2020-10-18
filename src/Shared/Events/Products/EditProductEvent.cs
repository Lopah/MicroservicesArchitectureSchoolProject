using System;

namespace DemoApp.Shared.Events.Products
{
    public class EditProductEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
