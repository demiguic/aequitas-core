namespace AequitasTracker.Models
{
    public class Position
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime AcquisitionDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Asset Asset { get; set; } = null!;
    }
}