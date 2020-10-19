using System;

namespace DemoApp.Core.Models.Orders
{
    public class OrderProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}