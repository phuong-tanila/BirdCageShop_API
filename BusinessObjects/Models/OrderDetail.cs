using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class OrderDetail : BaseEntity
    {
        public OrderDetail()
        {
            FeedbackAttachments = new HashSet<FeedbackAttachment>();
        }

        public int Price { get; set; }
        public int Status { get; set; }
        public Guid OrderId { get; set; }
        public string? Content { get; set; }
        public DateTime? PostDate { get; set; }
        public int? Rating { get; set; }
        public int? Quantity { get; set; }
        public Guid CageId { get; set; }

        public virtual Cage Cage { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<FeedbackAttachment> FeedbackAttachments { get; set; }
    }
}
