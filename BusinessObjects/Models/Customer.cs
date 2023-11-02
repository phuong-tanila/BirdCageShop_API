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

        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Point { get; set; }
        [Required]
        [IgnoreDataMember]
        public string? AccountId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Account? Account { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cage>? CustomCages { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
