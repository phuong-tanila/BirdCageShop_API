using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.CageComponentDTOs
{
    public class CreateCageComponentModel
    {
        [Required]
        public Guid ComponentId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Component quantity must be between {1} and {2}")]
        public int Quantity { get; set; }
    }
}