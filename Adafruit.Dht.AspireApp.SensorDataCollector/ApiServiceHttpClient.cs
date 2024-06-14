using Adafruit.Dht.AspireApp.Models;
using System.Text;
using System.Text.Json;

namespace Adafruit.Dht.AspireApp.SensorDataCollector
{
    public class ApiServiceHttpClient
    {
        private readonly HttpClient _client;
        public ApiServiceHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task PostSensorReadingsAsync(List<DhtReading> readings, int maxItems = 10, CancellationToken cancellationToken = default)
        {
            // Serialize the data object to JSON
            var jsonData = JsonSerializer.Serialize(readings);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _client.PostAsync("/sensor/readings", content, cancellationToken);

            // Optionally, handle the response
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {responseBody}");
            }
        }
    }
}

