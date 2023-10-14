using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class FeedbackAttachment
    {
        public string Id { get; set; } = null!;
        public string? Path { get; set; }
        public string OrderDetailId { get; set; } = null!;

        public virtual OrderDetail OrderDetail { get; set; } = null!;
    }
}
