using System.ComponentModel.DataAnnotations;

namespace DemoApp.Web.Models.Users
{
    public class CreateUserViewModel: BaseViewModel
    {
        [MinLength(2)]
        public string Username { get; set; }
        [MinLength(2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
