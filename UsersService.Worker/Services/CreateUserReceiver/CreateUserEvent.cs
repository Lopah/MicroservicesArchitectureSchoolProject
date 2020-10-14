namespace UsersService.Worker.Services.CreateUserReceiver
{
    public class CreateUserEvent
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}