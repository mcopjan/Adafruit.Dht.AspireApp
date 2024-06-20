using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Adafruit.Dht.AspireApp.Models
{
    public class DhtReadingContext(DbContextOptions<DhtReadingContext> options) : DbContext(options)
    {
        public DbSet<DhtReading> SensorReadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DhtReading>(entity =>
            {
                entity.ToTable("SensorReadings"); // Explicitly specify the table name

                entity.Property(e => e.CreatedLocal)
                    .HasConversion(
                        v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                        v => DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                    );
            });
        }
    }
}
