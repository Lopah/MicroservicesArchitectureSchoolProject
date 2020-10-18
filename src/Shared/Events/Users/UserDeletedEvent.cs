using System;

namespace DemoApp.Shared.Events.Users
{
    public class UserDeletedEvent
    {
        public Guid Id { get; set; }
    }
}