using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Voucher : BaseEntity
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }

        public string Title { get; set; } = null!;
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int? ConditionPoint { get; set; }
        public int Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
