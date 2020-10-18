using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApp.Core.Models.Users;

namespace DemoApp.Core.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
    }
}