using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsService.Infrastructure.Data;

namespace ProductsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ProductsController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        // GET: api/Products
        [HttpGet]
        public  async Task<IActionResult> Get()
        {
            var products = await applicationDbContext.Products.ToListAsync();

            return Json(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await applicationDbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
                return BadRequest();

            return Json(product);
        }
    }
}
