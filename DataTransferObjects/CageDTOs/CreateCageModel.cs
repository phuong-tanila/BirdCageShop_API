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
        public string Name { get; set; }

        public string Description { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int InStock { get; set; }

        public int Price { get; set; }
        [FirebaseUrl]
        public string ImagePath { get; set; }

        public List<CreateImageModel> Images { get; set; }

        public ISet<CreateCageComponentModel> CageComponents { get; set; }

    }
}
