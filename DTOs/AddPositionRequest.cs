using System.ComponentModel.DataAnnotations;

namespace AequitasTracker.DTOs
{
    public class AddPositionRequest
    {
        [Required(ErrorMessage = "O símbolo (Ticker) é obrigatório.")]
        public required string Ticker { get; set; }

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public required decimal Quantity { get; set; }

        [Required(ErrorMessage = "O preço de compra é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço de compra deve ser positivo.")]
        public decimal PurchasePrice { get; set; }
    }
}