using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class FeedbackDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Invalid {0}")]
        public int? Rating { get; set; }
        [Required]
        [StringLength(100)]
        public string? Content { get; set; }
        public DateTime? PostDate { get; set; }
        public Guid CageId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
    }
}
