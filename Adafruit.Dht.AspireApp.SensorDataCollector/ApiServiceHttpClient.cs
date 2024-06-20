using Adafruit.Dht.AspireApp.Models;
using System.Text;
using System.Text.Json;

namespace Adafruit.Dht.AspireApp.SensorDataCollector
{
    public class ApiServiceHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<ApiServiceHttpClient> _logger;

        public ApiServiceHttpClient(HttpClient client, ILogger<ApiServiceHttpClient> logger)
        {
            _client = client;
            _logger = logger;
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

                _logger.LogError($"An error occurred while sending data from worker service data collector to API service {response.StatusCode} {responseBody}");
            }
        }
    }
}

