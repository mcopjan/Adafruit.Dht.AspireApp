using Adafruit.Dht.AspireApp.Models;
using System.Text.Json;

namespace Adafruit.Dht.AspireApp.SensorDataCollector;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ApiServiceHttpClient _apiServiceHttpClient;
    private static readonly HttpClient _sensorHttpClient = new HttpClient() { BaseAddress = new Uri("http://192.168.5.208:5000/") };
    private List<DhtReading> readings = new List<DhtReading>();
    private readonly Timer _timer;

    public Worker(ApiServiceHttpClient httpClient, ILogger<Worker> logger)
    {
        _logger = logger;
        _apiServiceHttpClient = httpClient;
        _timer = new Timer(OnTimedEvent, _apiServiceHttpClient, 0, 30000);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                var response = await _sensorHttpClient.GetStringAsync("sensor");
                var reading = JsonSerializer.Deserialize<DhtReading>(response);
                reading.CreatedLocal = DateTime.Now;
                readings.Add(reading);
                _logger.LogInformation($"DHT sensor readings captured: {reading.Temperature}C, {reading.Humidity}%");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }

    private async void OnTimedEvent(Object state)
    {
        if (!readings.Any())
        {
            _logger.LogWarning("No sensor data to send.");
        }
        else
        {
            try
            {
                await ((ApiServiceHttpClient)state).PostSensorReadingsAsync(readings);
                readings.Clear();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message, ex);
            }
            
        }
    }
}
