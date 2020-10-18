using System;

namespace OrdersService.Infrastructure.Data.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
