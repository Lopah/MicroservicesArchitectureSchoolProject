using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.Shared.Events.Products
{
    public class ProductEditedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
