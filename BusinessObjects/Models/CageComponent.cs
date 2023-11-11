using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public partial class CageComponent : BaseEntity
    {
        public int Quantity { get; set; }
        public Guid? ComponentId { get; set; }
        public Guid? CageId { get; set; }
        [JsonIgnore]
        public virtual Cage? Cage { get; set; }
        public virtual Component? Component { get; set; }
    }
}
