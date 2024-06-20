using Adafruit.Dht.AspireApp.Models;
using Microsoft.EntityFrameworkCore;
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


        builder.Services.AddDbContextPool<DhtReadingContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));

        

        // Add services to the container.
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();



        app.MapGet("/sensor/readings", async (DhtReadingContext context) =>
        {
            try
            {
                var entries = await context.SensorReadings.ToListAsync();
                return entries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //logger.LogError(ex.Message, ex);
            }
            return null;
            
        });



        app.MapPost("/sensor/readings", async (HttpRequest request, DhtReadingContext context) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var readings = JsonSerializer.Deserialize<List<DhtReading>>(body);

            if (readings != null)
            {
                try
                {
                    context.Database.EnsureCreated();
                    var readingToEnter = readings.First();
                    //readings.ForEach(r => r.Id = Guid.NewGuid());
                    await context.SensorReadings.AddRangeAsync(readings);
                    await context.SaveChangesAsync();

                    var entries = await context.SensorReadings.ToListAsync();

                    Console.WriteLine($"number of entries in DB={entries.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //logger.LogError(ex.Message, ex);
                }
            }
            return Results.Ok();
        });

        app.MapDefaultEndpoints();

        app.Run();
    }
}