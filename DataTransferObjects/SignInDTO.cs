using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects
{
    public class SignInDTO
    {
        [Required]
        public string Phone { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
