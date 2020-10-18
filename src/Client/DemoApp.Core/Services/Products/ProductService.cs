using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DemoApp.Core.Config;
using DemoApp.Core.Models.Products;
using DemoApp.Core.Models.Users;
using DemoApp.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace DemoApp.Core.Services.Products
{
    public class ProductService: IProductService
    {
        private const string HttpClientName = "ProductServiceHttpClient";
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public ProductService(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient(HttpClientName);
            client.BaseAddress = new Uri(_appSettings.Services.ProductServiceUrl);

            return client;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            var client = this.GetClient();
            var response = await client.GetAsync("api/products");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Product service not available.");
            }

            var result = await response.Content.ReadAsStringAsync();

            return result.Deserialize<List<ProductDto>>();
        }

        public async Task<ProductDto> GetProductAsync(Guid id)
        {
            var client = this.GetClient();
            var response = await client.GetAsync($"api/products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Product service not available.");
            }

            var result = await response.Content.ReadAsStringAsync();

            return result.Deserialize<ProductDto>();
        }
    }
}