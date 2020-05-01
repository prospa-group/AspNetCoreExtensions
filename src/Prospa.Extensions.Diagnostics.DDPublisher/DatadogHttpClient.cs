using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Prospa.Extensions.Diagnostics.DDPublisher
{
    public class DatadogHttpClient
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public DatadogHttpClient(DatadogConfiguration config)
        {
            _client = new HttpClient();
            _url = $"{config.Url}/v1/series?api_key={config.ApiKey}&application_key={config.ApplicationKey}";
        }

        public async Task<string> SendMetricsAsync(DataDogMetric metric)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _url)
            {
                Content = new StringContent(JsonSerializer.Serialize(metric, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }))
            };

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
