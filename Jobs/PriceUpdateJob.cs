using AequitasTracker.Data;
using AequitasTracker.Models;
using AequitasTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace AequitasTracker.Jobs
{
    public class PriceUpdateJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AlphaVantageService _alphaVantageService;

        public PriceUpdateJob(ApplicationDbContext dbContext, AlphaVantageService alphaVantageService)
        {
            _dbContext = dbContext;
            _alphaVantageService = alphaVantageService;
        }

        /// <summary>
        /// Executa a atualização de preços para todos os ativos na base de dados.
        /// </summary>
        public async Task ExecutePriceUpdateAsync()
        {
            Console.WriteLine($"[{DateTime.Now}] -> Iniciando o Price Update Job...");
            var uniqueTickers = await _dbContext.Assets
                .Select(a => a.Ticker)
                .Distinct()
                .ToListAsync();

            if (!uniqueTickers.Any())
            {
                Console.WriteLine("Nenhum ativo encontrado para atualização de preços.");
                return;
            }

            Console.WriteLine($"Ativos a serem atualizados: {string.Join(", ", uniqueTickers)}");

            foreach (var ticker in uniqueTickers)
            {
                var currentPrice = await _alphaVantageService.GetCurrentPriceAsync(ticker);
                if (currentPrice.HasValue)
                {
                    var asset = await _dbContext.Assets.FirstOrDefaultAsync(a => a.Ticker == ticker);

                    if (asset != null)
                    {
                        var priceUpdate = new PriceUpdate
                        {
                            AssetId = asset.Id,
                            Price = currentPrice.Value,
                            Timestamp = DateTime.UtcNow
                        };

                        _dbContext.PriceUpdates.Add(priceUpdate);
                        Console.WriteLine($"Preço de {ticker} ({currentPrice.Value:C}) registado com sucesso.");
                    }
                }
                // API RATE LIMIT: ONE REQUEST PER MINUTE OR 25 PER DAY
                await Task.Delay(TimeSpan.FromMilliseconds(1500));
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}