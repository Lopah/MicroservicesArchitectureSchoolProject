using System.Collections.Generic;
using DemoApp.Core.Models.Users;

namespace DemoApp.Web.Models.Users
{
    public class UsersViewModel
    {
        public UsersViewModel(List<UserDto> users)
        {
            this.Users = users;
        }
        public List<UserDto> Users { get; private set; }
    }
}