using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models
{
    public partial class OrderDetail : BaseEntity
    {
        public OrderDetail()
        {
            FeedbackAttachments = new HashSet<FeedbackAttachment>();
        }

        public int Price { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Invalid {0}")]
        public int Quantity { get; set; }
        [Range(1, 5, ErrorMessage = "Invalid {0}")]
        public int? Rating { get; set; }
        public string? Content { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid OrderId { get; set; }
        [Required]
        public Guid CageId { get; set; }

        public virtual Cage? Cage { get; set; }
        public virtual Order? Order { get; set; }
        public virtual ICollection<FeedbackAttachment> FeedbackAttachments { get; set; }
    }
}
