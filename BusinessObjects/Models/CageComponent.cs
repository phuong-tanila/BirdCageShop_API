using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class CageComponent : BaseEntity
    {
        public Guid ComponentId { get; set; }
        public int? Quantity { get; set; }

        public virtual Cage Cage { get; set; } = null!;
        public virtual Component Component { get; set; } = null!;
    }
}
