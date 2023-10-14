using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Point { get; set; }
        public string? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
