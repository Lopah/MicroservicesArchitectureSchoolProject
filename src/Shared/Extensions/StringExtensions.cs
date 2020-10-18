using System.Text.Json;

namespace DemoApp.Shared.Extensions
{
    public static class StringExtensions
    {
        private static JsonSerializerOptions GetDefaultSettings()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private static JsonSerializerOptions DefaultSettings => GetDefaultSettings();

        public static T Deserialize<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, DefaultSettings);
        }
    }
}