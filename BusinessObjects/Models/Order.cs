using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? Total { get; set; }
        public int? ShipFee { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
