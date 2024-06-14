using Adafruit.Dht.AspireApp.Models;

namespace Adafruit.Dht.AspireApp.Web;

public class DhtSensorReadingsApiClient(HttpClient httpClient)
{
    public async Task<DhtReading[]> GetReadingsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<DhtReading>? readings = null;

        await foreach (var reading in httpClient.GetFromJsonAsAsyncEnumerable<DhtReading>("/sensor/readings", cancellationToken))
        {
            if (readings?.Count >= maxItems)
            {
                break;
            }
            if (reading is not null)
            {
                readings ??= [];
                readings.Add(reading);
            }
        }

        return readings?.ToArray() ?? [];
    }
}