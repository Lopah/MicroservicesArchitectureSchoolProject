using System;

namespace DemoApp.Shared.Events.Users
{
    public class UserCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}