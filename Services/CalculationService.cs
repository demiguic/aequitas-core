using Microsoft.EntityFrameworkCore;
using AequitasTracker.Data;
using AequitasTracker.DTOs;
using AequitasTracker.Models;

namespace AequitasTracker.Services
{
    public class CalculationService
    {
        private readonly ApplicationDbContext _dbContext;

        public CalculationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Calcula as métricas de performance e risco para todo o portfólio.
        /// </summary>
        public async Task<List<AssetPerformanceDTO>> CalculatePortfolioPerformanceAsync()
        {
            var positions = await _dbContext.Positions
                .Include(p => p.Asset)
                .ToListAsync();

            if (!positions.Any()) return new List<AssetPerformanceDTO>();

            var results = new List<AssetPerformanceDTO>();
            decimal totalPortfolioMarketValue = 0;


            foreach (var pos in positions)
            {
                var priceHistory = await _dbContext.PriceUpdates
                            .Where(p => p.AssetId == pos.AssetId)
                            .OrderByDescending(p => p.Timestamp)
                            .Take(2)
                            .ToListAsync();

                var currentPriceRecord = priceHistory.FirstOrDefault();
                var currentPrice = currentPriceRecord?.Price ?? pos.AveragePrice;

                var basePrice = pos.AveragePrice;

                if (priceHistory.Count > 1)
                {
                    basePrice = priceHistory[1].Price;
                }
                else if (priceHistory.Count == 1)
                {
                    basePrice = pos.AveragePrice;
                }


                var totalCost = pos.Quantity * pos.AveragePrice;
                var marketValue = pos.Quantity * currentPrice;

                decimal dailyPerformance = 0;
                if (basePrice > 0 && currentPrice != basePrice)
                {
                    dailyPerformance = ((currentPrice / basePrice) - 1M) * 100M;
                }

                totalPortfolioMarketValue += marketValue;

                results.Add(new AssetPerformanceDTO
                {
                    Ticker = pos.Asset.Ticker,
                    Quantity = pos.Quantity,
                    AveragePrice = pos.AveragePrice,
                    CurrentPrice = currentPrice,
                    MarketValue = marketValue,
                    TotalCost = totalCost,
                    ProfitAndLoss = marketValue - totalCost,
                    DailyPerformance = dailyPerformance,
                    ExposurePercentage = 0
                });
            }

            foreach (var result in results)
            {
                if (totalPortfolioMarketValue > 0)
                {
                    result.ExposurePercentage = (result.MarketValue / totalPortfolioMarketValue) * 100M;
                }
            }

            return results;
        }
    }
}