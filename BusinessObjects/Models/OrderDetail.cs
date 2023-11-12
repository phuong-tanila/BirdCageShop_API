using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

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
        [StringLength(100)]
        public string? Content { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid OrderId { get; set; }
        [Required]
        public Guid CageId { get; set; }

        public virtual Cage? Cage { get; set; }
        [JsonIgnore]
        public virtual Order? Order { get; set; }
        public virtual ICollection<FeedbackAttachment> FeedbackAttachments { get; set; }
    }
}
