namespace AequitasTracker.Models
{
    public class PriceUpdate
    {
        public int Id {get;set;}
        public int AssetId { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        // Navigation property
        public Asset Asset { get; set; } = null!;
    }
}