using System.Collections.Generic;

namespace DemoApp.Web.Models
{
    public class UsersViewModel
    {
        public UsersViewModel(List<(int, string)> users)
        {
            this.Users = users;
        }
        public List<(int Id, string Name)> Users { get; private set; }
    }
}