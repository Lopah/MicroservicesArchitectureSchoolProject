using System;
using System.Collections.Generic;

namespace DemoApp.Infrastructure.SqlServer.DbEntities
{
    public class Order
    {
        public Order()
        {
            Products = new List<Product>( );
        }
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
