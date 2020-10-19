using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Core.Models.Orders;
using DemoApp.Core.Models.Products;
using DemoApp.Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoApp.Web.Models.Orders
{
    public class CreateOrderViewModel: BaseViewModel
    {
        public CreateOrderViewModel()
        {
        }

        public CreateOrderViewModel(List<OrderUserDto> users, List<OrderProductDto> products)
        {
            this.SetUserList(users);
            this.SetProductList(products);
        }

        public List<SelectListItem> UsersList { get; set; } = new List<SelectListItem>();
        public List<OrderProductDto> ProductsList { get; set; } = new List<OrderProductDto>();

        public Guid UserId { get; set; }
        public List<CreateOrderProductModel> Products { get; set; } = new List<CreateOrderProductModel>();

        public void SetUserList(List<OrderUserDto> users)
        {
            this.UsersList = users?.Select(u => new SelectListItem(u.Name, u.Id.ToString())).ToList();
        }

        public void SetProductList(List<OrderProductDto> products)
        {
            this.ProductsList = products;
        }
    }

    public class CreateOrderProductModel
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
    }
}