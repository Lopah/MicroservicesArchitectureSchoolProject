using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrdersService.API.Models;
using OrdersService.Infrastructure.Data.Entities;

namespace OrdersService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly ApplicationDbContext _context;

        public OrdersController(ILogger<OrdersController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                    .ThenInclude(l => l.Product)
                .ToListAsync();

            var result = orders.Select(MapToDto).ToList();
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<ActionResult<OrderUser>> GetOrderUsers()
        {
            var orderUsers = await _context.OrderUsers.ToListAsync();
            return Ok(orderUsers);
        }

        [HttpGet("products")]
        public async Task<ActionResult<OrderProduct>> GetOrderProducts()
        {
            var orderProducts = await _context.OrderProducts.ToListAsync();
            return Ok(orderProducts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
        {
            var order = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .ThenInclude(l => l.Product)
                .FirstOrDefaultAsync(e => e.Id == id);

            var result = MapToDto(order);
            return Ok(result);
        }


        [HttpGet("user/{id}")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersForUser(Guid id)
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .ThenInclude(l => l.Product)
                .Where(e => e.OrderUser.Id == id)
                .ToListAsync( );

            var result = orders.Select(MapToDto).ToList();
            return Ok(result);
        }

        private OrderDto MapToDto(Order order)
        {
            var dto = new OrderDto();
            dto.OrderId = order.Id;
            dto.UserId = order.OrderUserId;
            dto.User = order.OrderUser.Name;
            dto.TotalPrice = order.GetTotalPrice();
            dto.CreatedOn = order.CreatedOn;
            dto.Products = order.OrderProducts.Select(l => new OrderDto.ProductDto(l.ProductId, l.Product.Name, l.Product.Price, l.Amount)).ToList();

            return dto;
        }
    }
}
