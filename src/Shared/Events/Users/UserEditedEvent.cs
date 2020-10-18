using System;

namespace DemoApp.Shared.Events.Users
{
    public class UserEditedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}