using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
        public DateTime EffectiveDate { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        [Required]
        public int? ConditionPoint { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
