using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace BusinessObjects.Models
{
    public partial class Cage : BaseEntity
    {
        public Cage()
        {
            CageComponents = new HashSet<CageComponent>();
            Images = new HashSet<Image>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Name { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int InStock { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int Price { get; set; }
        public double Rating { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public virtual ICollection<CageComponent> CageComponents { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
