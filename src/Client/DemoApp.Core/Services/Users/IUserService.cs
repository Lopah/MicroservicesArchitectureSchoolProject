using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApp.Core.Models.Users;

namespace DemoApp.Core.Services.Users
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserAsync(Guid id);
    }
}