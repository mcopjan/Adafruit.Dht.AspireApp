using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Adafruit.Dht.AspireApp.SensorDataCollector
{
    public class DhtMetricsApiClient
    {
        private readonly HttpClient _client;
        public DhtMetricsApiClient(HttpClient client)
        {
            _client = client;
        }
        public async Task<WeatherForecast[]> GetWeatherAsync(int maxItems = 10, CancellationToken cancellationToken = default)
        {
            List<WeatherForecast>? forecasts = null;

            await foreach (var forecast in _client.GetFromJsonAsAsyncEnumerable<WeatherForecast>("/weatherforecast", cancellationToken))
            {
                if (forecasts?.Count >= maxItems)
                {
                    break;
                }
                if (forecast is not null)
                {
                    forecasts ??= [];
                    forecasts.Add(forecast);
                }
            }

            return forecasts?.ToArray() ?? [];
        }

        public async Task PostWeatherAsync(object data, int maxItems = 10, CancellationToken cancellationToken = default)
        {
            // Serialize the data object to JSON
            var jsonData = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await _client.PostAsync("/weatherforecast", content, cancellationToken);

            // Optionally, handle the response
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {responseBody}");
            }
        }
    }



    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
