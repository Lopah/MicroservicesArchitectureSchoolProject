using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersService.Infrastructure.Data;
using UsersService.Infrastructure.Data.Entities;

namespace UsersService.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly ApplicationDbContext _dbContext;

        public UsersController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }
    }
}