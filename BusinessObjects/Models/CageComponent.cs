using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class CageComponent : BaseEntity
    {
        public int Quantity { get; set; }
        public Guid? ComponentId { get; set; }
        public Guid? CageId { get; set; }

        public virtual Cage? Cage { get; set; }
        public virtual Component? Component { get; set; }
    }
}
