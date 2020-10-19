using System;
using System.Collections.Generic;

namespace OrdersService.API.Models
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
            public ProductDto(Guid id, string name, decimal price, int amount)
            {
                Id = id;
                Name = name;
                Price = price;
                Amount = amount;
            }

            public Guid Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Amount { get; set; }
        }
    }
}