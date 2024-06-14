using System.Text.Json.Serialization;


namespace Adafruit.Dht.AspireApp.Models
{
    public class DhtReading
    {
        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }
        [JsonPropertyName("created_local")]
        public DateTime? CreatedLocal { get; set; }
    }
}
