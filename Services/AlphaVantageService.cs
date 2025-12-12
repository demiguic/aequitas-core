using System.Text.Json;
using AequitasTracker.AlphaVantageModels;
using Microsoft.Extensions.Configuration;

namespace AequitasTracker.Services
{
    public class AlphaVantageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol={0}&apikey={1}";

        public AlphaVantageService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["AlphaVantage:ApiKey"] ?? throw new InvalidOperationException("AlphaVantage API Key não configurada.");
        }

        /// <summary>
        /// Obtém a cotação em tempo real (ou a última disponível) para um determinado símbolo.
        /// </summary>
        /// <param name="ticker">O símbolo do ativo (ex: TSLA).</param>
        /// <returns>O preço atualizado ou null se a chamada falhar.</returns>
        public async Task<decimal?> GetCurrentPriceAsync(string ticker)
        {
            var url = string.Format(BaseUrl, ticker, _apiKey);

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();

                var quoteResponse = JsonSerializer.Deserialize<GlobalQuoteResponse>(json);

                if (quoteResponse?.GlobalQuote?.Price != null)
                {
                    if (decimal.TryParse(
                        quoteResponse.GlobalQuote.Price,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out var price))
                    {
                        Console.WriteLine($"Preço do {ticker} obtido: {price:C2}"); 
                        return price;
                    }
                }

                Console.WriteLine($"Aviso: Não foi possível obter ou analisar o preço para o ticker: {ticker}");
                return null;
            }
            catch (HttpRequestException ex)
            {
                // Tratamento de erros de rede ou HTTP
                Console.WriteLine($"Erro ao chamar a API da Alpha Vantage para {ticker}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                // Tratamento de erros de desserialização JSON
                Console.WriteLine($"Erro de JSON ao processar a resposta para {ticker}: {ex.Message}");
                return null;
            }
        }
    }
}