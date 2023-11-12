using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public partial class Voucher : BaseEntity
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Invalid {0}")]
        public double Discount { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid {0}")]
        public int? ConditionPoint { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
