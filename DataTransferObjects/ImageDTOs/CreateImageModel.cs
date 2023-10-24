using DataTransferObjects.Commons.ValidatorAttributes;

namespace DataTransferObjects.ImageDTOs
{
    public class CreateImageModel
    {
        [FirebaseUrl]
        public string ImagePath { get; set; }
    }
}