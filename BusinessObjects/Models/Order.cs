using BusinessObjects.Helper;
using FluentValidation.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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
        public DateTime? DeliveryDate { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        public Guid? VoucherId { get; set; }
        [Required]
        public Guid? CustomerId { get; set; }
        public int? Total { get; set; }
        [Required]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid {0}")]
        public int? ShipFee { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
