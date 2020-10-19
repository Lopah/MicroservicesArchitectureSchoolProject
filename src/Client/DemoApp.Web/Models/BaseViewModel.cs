using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DemoApp.Web.Models
{
    public abstract class BaseViewModel
    {
        public bool ShowSuccess { get; set; } = false;
    }
}