using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        [StringLength(50)]
        public string? LastName { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Point { get; set; }
        public string? AccountId { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }

        [JsonIgnore]
        public virtual Account? Account { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cage>? CustomCages { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
