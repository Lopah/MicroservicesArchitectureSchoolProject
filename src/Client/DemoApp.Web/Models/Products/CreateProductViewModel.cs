namespace DemoApp.Web.Models.Products
{
    public class CreateProductViewModel: BaseViewModel
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}