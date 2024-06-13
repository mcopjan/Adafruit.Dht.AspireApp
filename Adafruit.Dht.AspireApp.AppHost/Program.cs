var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Adafruit_Dht_AspireApp_ApiService>("apiservice");

builder.AddProject<Projects.Adafruit_Dht_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.Adafruit_Dht_AspireApp_SensorDataCollector>("adafruit-dht-aspireapp-sensordatacollector")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
