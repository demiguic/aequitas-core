using System.Text.Json.Serialization;

namespace AequitasTracker.AlphaVantageModels
{
    public class GlobalQuoteResponse
    {
        [JsonPropertyName("Global Quote")]
        public GlobalQuote? GlobalQuote { get; set; }
        // ADICIONAR ESTA PROPRIEDADE PARA CAPTURAR A MENSAGEM DE ERRO/AVISO
        [JsonPropertyName("Note")]
        public string? Note { get; set; }
        [JsonPropertyName("Information")]
        public string? Information { get; set; }
    }

    public class GlobalQuote
    {
        [JsonPropertyName("05. price")]
        public string? Price { get; set; }
        [JsonPropertyName("07. latest trading day")]
        public string? LatestTradingDay { get; set; }
        [JsonPropertyName("01. symbol")]
        public string? Symbol { get; set; }
    }
}