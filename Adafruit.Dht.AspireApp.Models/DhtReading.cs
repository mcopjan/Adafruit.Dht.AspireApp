using System.Text.Json.Serialization;


namespace Adafruit.Dht.AspireApp.Models
{
    public class DhtReading
    {
        public Guid Id { get; set; } = new Guid();    
        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }
        [JsonPropertyName("created_local")]
        private DateTime? createdLocal;
        public DateTime? CreatedLocal
        {
            get => createdLocal;
            set => createdLocal = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : (DateTime?)null;
        }
    }
}
