using System;
using DemoApp.Core.Models.Products;

namespace DemoApp.Web.Models.Products
{
    public class EditProductViewModel
    {

        public EditProductViewModel()
        {

        }
        public EditProductViewModel(ProductDto data)
        {
            this.Id = data.Id;
            this.Name = data.Name;
            this.Amount = data.Amount;
            this.Price = data.Price;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}