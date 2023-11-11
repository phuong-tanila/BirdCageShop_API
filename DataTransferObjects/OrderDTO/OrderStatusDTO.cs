using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class OrderStatusDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Range(1, 4)]
        public int Status { get; set; }
    }
}
