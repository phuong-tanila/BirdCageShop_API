using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Account
    {
        public Account()
        {
            Customers = new HashSet<Customer>();
        }

        public string Id { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Status { get; set; }
        public string Role { get; set; } = null!;

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
