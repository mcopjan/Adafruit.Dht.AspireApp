using Adafruit.Dht.AspireApp.SensorDataCollector;


var builder = Host.CreateApplicationBuilder(args);
// Add service defaults & Aspire components.
builder.AddServiceDefaults();


builder.Services.AddHttpClient<DhtMetricsApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://apiservice");
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run(); ;
