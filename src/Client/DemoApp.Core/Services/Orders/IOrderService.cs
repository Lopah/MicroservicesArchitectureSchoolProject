﻿using DemoApp.Core.Models.Orders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApp.Core.Services.Orders
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderAsync(Guid id);
    }
}
