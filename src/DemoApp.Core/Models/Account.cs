using System;
using System.Collections.Generic;

namespace DemoApp.Core.Models
{
    public class Account
    {
        public Account()
        {
            Orders = new List<Order>();
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get;}
    }
}
