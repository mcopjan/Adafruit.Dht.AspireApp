using Adafruit.Dht.AspireApp.Models;
using System.Collections.Concurrent;
using System.Text.Json;

internal class Program
{
    private static ConcurrentBag<DhtReading> SensorReadings = new ConcurrentBag<DhtReading>(); 
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service defaults & Aspire components.
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();



        app.MapGet("/sensor/readings", () =>
        {
            return SensorReadings;
        });

        app.MapPost("/sensor/readings", async (HttpRequest request) =>
        {
            // Read the request body
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var readings = JsonSerializer.Deserialize<List<DhtReading>>(body);
            readings?.ForEach(x => SensorReadings.Add(x));
            Console.WriteLine(body);
            return Results.Ok();
        });

        app.MapDefaultEndpoints();

        app.Run();
    }
}