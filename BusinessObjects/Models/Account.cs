
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

        public int Status { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
