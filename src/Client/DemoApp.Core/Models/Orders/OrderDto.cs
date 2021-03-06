using System;
using System.Collections.Generic;

namespace DemoApp.Core.Models.Orders
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string User { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();

        public class ProductDto
        {
            public ProductDto()
            {

            }

            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Amount { get; set; }
        }
    }
}