using System.Text.Json.Serialization;


namespace Adafruit.Dht.AspireApp.SensorDataCollector
{
    public class DhtReading
    {
        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }
    }
}
