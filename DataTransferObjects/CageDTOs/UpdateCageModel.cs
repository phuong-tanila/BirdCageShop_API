using DataTransferObjects.CageComponentDTOs;
using DataTransferObjects.Commons.ValidatorAttributes;
using DataTransferObjects.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.CageDTOs
{
    public class UpdateCageModel
    {
        public Guid Id { get; set; }
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

        public string Status { get; set; }
    }
}
