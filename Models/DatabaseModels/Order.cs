using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace SjxLogistics.Models.DatabaseModels
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string PickUp { get; set; }
        [Required]
        public string Delivery { get; set; }
        [Required]
        public string ReceiversName { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        [Required]
        public string ReceiversPhone { get; set; }

        public string DeliveryCode { get; set; }
        public string PaymentStatus { get; set; }

        [Required]
        public string Categories { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public string OrderCode { get; set; }

        [Required]
        //For References purposes 
        public string CustomersEmail { get; set; }

        [Required]
        //For References purposes 
        public DateTime CreatedAt { get; set; }

        //The day this item was picked Up
        public DateTime PickUpDate { get; set; }
        //The Time it was picked
        public DateTime PickUpTime { get; set; }

        //The day this item was Drop-ed Up
        public DateTime DeliveryDate { get; set; }
        //The Time it was Drop-ed
        public DateTime DeliveryTime { get; set; }
        /*
         * UnRequired
         */
        public string Status { get; set; }
        public float Charges { get; set; }
        public bool IsExpressDelivery { get; set; } = false;
        public string PaymentType { get; set; }
        public string Refno { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public virtual Users Users { get; set; }
    }
}