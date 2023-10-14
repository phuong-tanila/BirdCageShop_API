using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class FeedbackAttachment : BaseEntity
    {
        public string? Path { get; set; }
        public Guid OrderDetailId { get; set; }

        public virtual OrderDetail OrderDetail { get; set; } = null!;
    }
}
