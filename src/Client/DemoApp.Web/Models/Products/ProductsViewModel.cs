using System.Collections.Generic;
using DemoApp.Core.Models.Products;

namespace DemoApp.Web.Models.Products
{
    public class ProductsViewModel
    {
        public ProductsViewModel(List<ProductDto> data)
        {
            this.Products = data;
        }
        public List<ProductDto> Products { get; }
    }
}