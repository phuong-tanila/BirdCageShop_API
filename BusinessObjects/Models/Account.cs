
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Account : IdentityUser
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
        }

        //public string PhoneNumber { get; set; } = null!;
        //public string Password { get; set; } = null!;
        //public int Status { get; set; }
        //public string Role { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
