using System.Collections.Generic;
using System.Threading.Tasks;
using DemoApp.Core.Models.Products;
using DemoApp.Core.Models.Users;

namespace DemoApp.Core.Services.Products
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
    }
}