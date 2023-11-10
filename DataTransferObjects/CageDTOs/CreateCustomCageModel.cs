using DataTransferObjects.CageComponentDTOs;
using DataTransferObjects.Commons.ValidatorAttributes;
using DataTransferObjects.ImageDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.CageDTOs
{
    public class CreateCustomCageModel
    {

        [Required]
        public int Length { get; set; }
        [Required]
        public int Width { get; set; }
        [Required]
        public int Height { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public ISet<CreateCageComponentModel> CageComponents { get; set; }
    }
}
