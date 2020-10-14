﻿using System;
using System.Collections.Generic;

namespace DemoApp.Core.Models
{
    public class Order
    {
        public Order()
        {
            Products = new List<Product>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TotalPrice { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
