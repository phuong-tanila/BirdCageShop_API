using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.CageComponentDTOs;
using DataTransferObjects.Commons.ValidatorAttributes;
using DataTransferObjects.ImageDTOs;

namespace DataTransferObjects.CageDTOs
{
    public record CreateCageModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]

        public string Description { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]

        public int Length { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]

        public int Width { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]

        public int Height { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]

        public int InStock { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]

        public int Price { get; set; }
        [Required]
        [FirebaseUrl]
        public string ImagePath { get; set; }

        public List<CreateImageModel> Images { get; set; }

        public ISet<CreateCageComponentModel> CageComponents { get; set; }
        [Required]

        public string Status { get; set; }

    }
}
