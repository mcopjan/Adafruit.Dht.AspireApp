using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var db1 = builder.AddPostgres("sql1").WithPgAdmin().AddDatabase("postgres");




var apiService =
    builder.AddProject<Projects.Adafruit_Dht_AspireApp_ApiService>("apiservice")
    .WithReference(db1);

builder.AddProject<Projects.Adafruit_Dht_AspireApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.Adafruit_Dht_AspireApp_SensorDataCollector>("adafruit-dht-aspireapp-sensordatacollector")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
