using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Image : BaseEntity
    {
        public string ImagePath { get; set; } = null!;
        public Guid CageId { get; set; }

        public virtual Cage Cage { get; set; } = null!;
    }
}
