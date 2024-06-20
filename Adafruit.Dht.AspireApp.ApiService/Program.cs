using Adafruit.Dht.AspireApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
            //try
            //{
            //    if (!context.Database.CanConnect())
            //    {
            //        context.Database.EnsureCreated();
            //    }
            //    context.SaveChanges();
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine(ex.Message);
            //}
            

      
            
            
            return SensorReadings;
        });

        app.MapGet("/test", async (DhtReadingContext context) =>
        {
            try
            {
                //ADD ALL READINGS
                context.Database.EnsureCreated();
                await context.SensorReadings.AddAsync(new DhtReading() { Humidity = 60, Temperature = 21, CreatedLocal = DateTime.UtcNow });
                await context.SaveChangesAsync();

                //ar entries = await context.SensorReadings.ToListAsync();

                //Console.WriteLine($"number of entries in DB={entries.Count}");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }


        });


        app.MapPost("/sensor/readings", async (HttpRequest request, DhtReadingContext context) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            var readings = JsonSerializer.Deserialize<List<DhtReading>>(body);

            //if (readings != null)
            //{
            //    try
            //    {
            //       //ADD ALL READINGS
            //        await context.SensorReadings.AddAsync(readings.First());
            //        await context.SaveChangesAsync();

            //        var entries = await context.SensorReadings.ToListAsync();

            //        Console.WriteLine($"number of entries in DB={entries.Count}");
            //    }
            //    catch (Exception ex)
            //    {

            //        Console.WriteLine(  ex.Message);
            //    }
            //}
            return Results.Ok();
        });

        app.MapDefaultEndpoints();

        app.Run();
    }
}