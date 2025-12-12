using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AequitasTracker.Data;
using AequitasTracker.DTOs;
using AequitasTracker.Models;
using AequitasTracker.Services;

namespace AequitasTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly CalculationService _calculationService;
        public PositionsController(ApplicationDbContext dbContext, CalculationService calculationService)
        {
            _dbContext = dbContext;
            _calculationService = calculationService;
        }

        /// <summary>
        /// Adiciona uma nova posição ao portfólio.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddPosition([FromBody] AddPositionRequest request)
        {
            var asset = await _dbContext.Assets
                .FirstOrDefaultAsync(a => a.Ticker == request.Ticker.ToUpper());

            if (asset == null)
            {
                asset = new Asset { Ticker = request.Ticker.ToUpper() };
                _dbContext.Assets.Add(asset);
                await _dbContext.SaveChangesAsync();
            }

            var position = new Position
            {
                AssetId = asset.Id,
                Quantity = request.Quantity,
                AveragePrice = request.PurchasePrice,
                AcquisitionDate = DateTime.UtcNow
            };

            _dbContext.Positions.Add(position);
            await _dbContext.SaveChangesAsync();

            var responseDto = new PositionResponseDTO
            {
                Id = position.Id,
                Ticker = position.Asset.Ticker,
                Quantity = position.Quantity,
                AveragePrice = position.AveragePrice,
                AcquisitionDate = position.AcquisitionDate
            };

            return CreatedAtAction(nameof(GetPositions), new { id = position.Id }, responseDto);
        }

        /// <summary>
        /// Obtém todas as posições do portfólio.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPositions()
        {
            var positionsDto = await _dbContext.Positions
                    .Include(p => p.Asset)
                    .Select(p => new PositionResponseDTO
                    {
                        Id = p.Id,
                        Ticker = p.Asset.Ticker,
                        Quantity = p.Quantity,
                        AveragePrice = p.AveragePrice,
                        AcquisitionDate = p.AcquisitionDate
                    })
                    .ToListAsync();

            if (!positionsDto.Any())
            {
                return NotFound("Nenhuma posição encontrada.");
            }

            // Retornamos a lista de DTOs, que é segura para serialização.
            return Ok(positionsDto);
        }
        /// <summary>
        /// Obtém as métricas de performance e risco do portfólio.
        /// </summary>
        [HttpGet("performance")]
        public async Task<IActionResult> GetPortfolioPerformance()
        {
            var performance = await _calculationService.CalculatePortfolioPerformanceAsync();

            if (!performance.Any())
            {
                return NotFound("Nenhuma posição encontrada para calcular a performance.");
            }

            return Ok(performance);
        }
    }
}