using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Description { get; set; }
        public string Address { get; set; } = null!;
        public string? VoucherId { get; set; }
        public string CustomerId { get; set; } = null!;
        public int? Total { get; set; }
        public int? ShipFee { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
