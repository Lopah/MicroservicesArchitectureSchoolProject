using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdersService.Infrastructure.Data.Entities
{
    public class Order
    {
        public Order()
        {
            OrderProducts = new List<OrderProduct>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public OrderUser OrderUser { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public int TotalPrice => OrderProducts.Sum(e => e.Amount);

    }
}
