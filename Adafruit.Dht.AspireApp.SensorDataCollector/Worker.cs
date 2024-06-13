using System.Text.Json;

namespace Adafruit.Dht.AspireApp.SensorDataCollector;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly DhtMetricsApiClient serviceApiHttpClient;
    private static readonly HttpClient _client = new HttpClient() { BaseAddress = new Uri("http://192.168.5.208:5000/") };

    public Worker(DhtMetricsApiClient httpClient, ILogger<Worker> logger)
    {
        _logger = logger;
        serviceApiHttpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        DhtReading readings = new DhtReading();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                var response = await _client.GetStringAsync("sensor");
                readings = JsonSerializer.Deserialize<DhtReading>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                readings.Humidity = 0;
                readings.Temperature = 0;
            }

            await serviceApiHttpClient.PostWeatherAsync(readings);
            await Task.Delay(10000, stoppingToken);
        }
    }
}
