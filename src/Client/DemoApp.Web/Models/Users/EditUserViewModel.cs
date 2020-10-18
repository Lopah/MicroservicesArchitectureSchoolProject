using System;

namespace DemoApp.Web.Models.Users
{
    public class EditUserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}