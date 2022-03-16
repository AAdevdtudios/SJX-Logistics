using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.Request
{
    public class OrderRequest : QuoteRequest
    {
        [Required]
        public string PickUp { get; set; }
        [Required]
        public string Delivery { get; set; }
        [Required]
        public string ReceiversName { get; set; }
        [Required]
        public string Categories { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public string ReceiversPhone { get; set; }

        public bool IsExpressDelivery { get; set; } = false;
    }
}
