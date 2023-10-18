using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BusinessObjects.Models
{
    public partial class Component : BaseEntity
    {
        public Component()
        {
            CageComponents = new HashSet<CageComponent>();
        }
        [Required]
        public string? Name { get; set; }
        [Required]
        public int? Price { get; set; }
        [Required]
        public string? ImagePath { get; set; }
        [Required]
        public string? Type { get; set; }
        [IgnoreDataMember]
        public virtual ICollection<CageComponent>? CageComponents { get; set; }
    }
}
