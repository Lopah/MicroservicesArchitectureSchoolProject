using System;
using System.Collections.Generic;

namespace OrdersService.Infrastructure.Data.Entities
{
    public class Order
    {
        public Order()
        {
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalPrice { get; set; }
    }
}
