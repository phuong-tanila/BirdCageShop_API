using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Image
    {
        public string Id { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public string CageId { get; set; } = null!;

        public virtual Cage Cage { get; set; } = null!;
    }
}
