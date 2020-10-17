using System;
using System.Collections.Generic;
using DemoApp.Infrastructure.SqlServer.DbEntities;

namespace UsersService.Infrastructure.Data.Entites
{
    public class User
    {
        public User()
        {
            Orders = new List<Order>( );
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; }
    }
}
