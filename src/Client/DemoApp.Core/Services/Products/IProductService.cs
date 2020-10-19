using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApp.Core.Models.Products;

namespace DemoApp.Core.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductAsync(Guid id);
        Task DeleteProductAsync(Guid id);
    }
}