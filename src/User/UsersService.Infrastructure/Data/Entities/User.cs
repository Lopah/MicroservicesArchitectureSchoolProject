using System;

namespace UsersService.Infrastructure.Data.Entities
{
    public class User
    {
        public User()
        {
        }
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
