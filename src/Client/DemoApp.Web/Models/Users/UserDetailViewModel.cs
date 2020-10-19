using System;
using System.Collections.Generic;
using DemoApp.Core.Models.Orders;
using DemoApp.Core.Models.Users;

namespace DemoApp.Web.Models.Users
{
    public class UserDetailViewModel: BaseViewModel
    {
        public UserDetailViewModel(UserDto user, List<OrderDto> orders)
        {
            Id = user.Id;
            Name = user.Name;
            Username = user.Username;
            Orders = orders ?? new List<OrderDto>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

        public List<OrderDto> Orders { get; set; }
    }
}
