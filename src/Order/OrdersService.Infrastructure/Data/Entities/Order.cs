using System;
using System.Collections.Generic;
using System.Linq;

namespace OrdersService.Infrastructure.Data.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid OrderUserId { get; set; }
        public OrderUser OrderUser { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public ICollection<OrderProductLink> OrderProducts { get; set; } = new List<OrderProductLink>();

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            if (OrderProducts.Count > 0)
            {
                foreach (var link in OrderProducts)
                {
                    if (link.Product != null)
                    {
                        totalPrice += link.Amount * link.Product.Price;
                    }
                }
            }

            return totalPrice;
        }
    }
}
