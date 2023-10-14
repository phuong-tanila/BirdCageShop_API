using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Point { get; set; }
        public string AccountId { get; set; } = null!;

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
