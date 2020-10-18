using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrdersService.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .ToListAsync();

            return Ok(orders);
        }

        public async Task<IActionResult> GetOrderForUser(Guid userId)
        {
            var orders = await _context.Orders
                .Include(e => e.OrderUser)
                .Include(e => e.OrderProducts)
                .Where(e => e.OrderUser.Id == userId)
                .ToListAsync( );

            return Ok(orders);
        }
    }
}
