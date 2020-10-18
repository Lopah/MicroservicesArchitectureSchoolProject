using DemoApp.Core.Config;
using DemoApp.Core.Models.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace DemoApp.Core.Services.Orders
{
    public class OrderService : IOrderService
    {
        private const string HttpClientName = "UserServiceHttpClient";
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public OrderService(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var client = this.GetClient();
            var response = await client.GetAsync("api/orders");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("User service not available.");
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<OrderDto>>(result);
        }

        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient(HttpClientName);
            client.BaseAddress = new Uri(_appSettings.Services.UserServiceUrl);

            return client;
        }
    }
}
