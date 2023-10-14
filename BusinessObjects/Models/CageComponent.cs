using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class CageComponent
    {
        public string CageId { get; set; } = null!;
        public string ComponentId { get; set; } = null!;
        public int? Quantity { get; set; }

        public virtual Cage Cage { get; set; } = null!;
        public virtual Component Component { get; set; } = null!;
    }
}
