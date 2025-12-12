namespace AequitasTracker.Models
{
    public class Asset
    {
        public int Id{ get; set; }
        public required string Ticker { get; set; }
        public string? Name { get; set; }
        
        // Navigation properties
        public ICollection<Position> Positions { get; set; } = new List<Position>();
        public ICollection<PriceUpdate> PriceUpdates { get; set; } = new List<PriceUpdate>();
    }
}