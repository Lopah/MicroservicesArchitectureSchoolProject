using System.Collections.Generic;
using DemoApp.Core.Models.Orders;

namespace DemoApp.Web.Models.Orders
{
    public class OrdersViewModel
    {
        public OrdersViewModel(List<OrderDto> orders)
        {
            Orders = orders;
        }
        public List<OrderDto> Orders { get; }
    }
}