using System;
using System.ComponentModel.DataAnnotations;
using DemoApp.Core.Models.Users;

namespace DemoApp.Web.Models.Users
{
    public class EditUserViewModel: BaseViewModel
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
        [MinLength(2)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Name { get; set; }
    }
}