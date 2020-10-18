using System;
using DemoApp.Core.Models.Users;

namespace DemoApp.Web.Models.Users
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {

        }
        public EditUserViewModel(UserDto data)
        {
            this.Id = data.Id;
            this.Username = data.Username;
            this.Password = data.Password;
            this.Name = data.Password;
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}