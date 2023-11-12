﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        //[IgnoreDataMember]
        public bool? IsDeleted { get; set; } = false;
    }
}
