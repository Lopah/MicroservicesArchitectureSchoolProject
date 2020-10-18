namespace DemoApp.Shared.Events.Products
{
    public class CreateProductEvent
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
