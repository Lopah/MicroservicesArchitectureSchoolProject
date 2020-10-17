using System;

namespace DemoApp.Infrastructure.Events
{
    public class UserCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}