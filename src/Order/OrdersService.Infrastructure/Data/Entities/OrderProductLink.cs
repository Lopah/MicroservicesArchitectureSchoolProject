using System;

namespace OrdersService.Infrastructure.Data.Entities
{
    public class OrderProductLink
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public OrderProduct Product { get; set; }

        public int Amount { get; set; }
    }
}