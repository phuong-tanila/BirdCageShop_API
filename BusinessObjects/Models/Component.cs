using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Component
    {
        public Component()
        {
            CageComponents = new HashSet<CageComponent>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? ImagePath { get; set; }

        public virtual ICollection<CageComponent> CageComponents { get; set; }
    }
}
