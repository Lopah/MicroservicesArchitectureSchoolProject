using System;

namespace DemoApp.Shared.Events.Users
{
    public class EditUserEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}