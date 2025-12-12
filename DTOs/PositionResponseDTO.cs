namespace AequitasTracker.DTOs
{
    public class PositionResponseDTO
    {
        public int Id { get; set; }
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public DateTime AcquisitionDate { get; set; }
    }
}