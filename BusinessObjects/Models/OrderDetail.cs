using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            FeedbackAttachments = new HashSet<FeedbackAttachment>();
        }

        public string Id { get; set; } = null!;
        public int Price { get; set; }
        public int Status { get; set; }
        public string OrderId { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime? PostDate { get; set; }
        public int? Rating { get; set; }
        public int? Quantity { get; set; }
        public string CageId { get; set; } = null!;

        public virtual Cage Cage { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual ICollection<FeedbackAttachment> FeedbackAttachments { get; set; }
    }
}
