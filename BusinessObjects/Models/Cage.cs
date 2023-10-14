using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Cage
    {
        public Cage()
        {
            CageComponents = new HashSet<CageComponent>();
            Images = new HashSet<Image>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public int? InStock { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int Price { get; set; }
        public string Status { get; set; } = null!;
        public int? Rating { get; set; }
        public string? ImagePath { get; set; }

        public virtual ICollection<CageComponent> CageComponents { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
