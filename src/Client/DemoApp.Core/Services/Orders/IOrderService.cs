﻿using DemoApp.Core.Models.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApp.Core.Services.Orders
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetOrdersForUserAsync(Guid id);
        Task<List<OrderUserDto>> GetUsersAsync();
        Task<List<OrderProductDto>> GetProductsAsync();
        Task<OrderDto> GetOrderAsync(Guid id);
    }
}
