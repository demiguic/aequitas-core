namespace AequitasTracker.DTOs
{
    public class AssetPerformanceDTO
    {
        public required string Ticker { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketValue { get; set; } // Valor de Mercado: Quantity * CurrentPrice
        public decimal TotalCost { get; set; }     // Custo Total: Quantity * AveragePrice
        public decimal ProfitAndLoss { get; set; } // P&L: MarketValue - TotalCost
        public decimal DailyPerformance { get; set; } // % de mudança do preço em relação ao dia anterior (vamos simplificar por enquanto)
        public decimal ExposurePercentage { get; set; } // Peso no portfólio
    }
}