using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Component : BaseEntity
    {
        public Component()
        {
            CageComponents = new HashSet<CageComponent>();
        }

        public string Name { get; set; }
        public int Price { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<CageComponent> CageComponents { get; set; }
    }
}
