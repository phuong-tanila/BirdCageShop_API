using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models
{
    public partial class FeedbackAttachment : BaseEntity
    {
        [Required]
        public string? Path { get; set; }
        [Required]
        public Guid? OrderDetailId { get; set; }

        public virtual OrderDetail? OrderDetail { get; set; }
    }
}
