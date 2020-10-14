using System;

namespace UsersService.Worker.Services.UserCreatedPublisher
{
    public class UserCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}