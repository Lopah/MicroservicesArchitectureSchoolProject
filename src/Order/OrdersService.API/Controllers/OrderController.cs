using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrdersService.Infrastructure.Data.Entities;

namespace OrdersService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ApplicationDbContext _context;

        public OrderController(ILogger<OrderController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Order>>> GetOrder(Guid id)
        {
            var order = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .FirstOrDefaultAsync(e => e.Id == id);

            return Ok(order);
        }


        [HttpGet("/user/{id}")]
        public async Task<ActionResult<List<Order>>> GetOrdersForUser(Guid id)
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .Where(e => e.OrderUser.Id == id)
                .ToListAsync( );

            return Ok(orders);
        }
    }
}
