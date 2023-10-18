using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models
{
    public partial class Component : BaseEntity
    {
        public Component()
        {
            CageComponents = new HashSet<CageComponent>();
        }

        public string Name { get; set; }
        [Required]
        public int? Price { get; set; }
        public string ImagePath { get; set; }

        public virtual ICollection<CageComponent> CageComponents { get; set; }
    }
}
