using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DemoApp.Core.Config;
using DemoApp.Core.Models.Users;
using DemoApp.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace DemoApp.Core.Services
{
    public class UserService: IUserService
    {
        private const string HttpClientName = "UserServiceHttpClient";
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _appSettings = appSettings.Value;
        }

        private HttpClient GetClient()
        {
            var client = _clientFactory.CreateClient(HttpClientName);
            client.BaseAddress = new Uri(_appSettings.Services.UserServiceUrl);

            return client;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var client = this.GetClient();
            var response = await client.GetAsync("api/users");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("User service not available.");
            }

            var result = await response.Content.ReadAsStringAsync();

            return result.Deserialize<List<UserDto>>();
        }
    }
}