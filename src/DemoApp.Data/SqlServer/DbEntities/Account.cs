using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Infrastructure.SqlServer.DbEntities
{
    public class Account
    {
        public Account()
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
