using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.CageDTOs
{
    public class UpdateCageCustomStatusModel
    {
        public string Status {  get; set; }

        public Guid CageId { get; set; }

        public int Price { get; set; }

        public string Description { get; set; }
    }
}
