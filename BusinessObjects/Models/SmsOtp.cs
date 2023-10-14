using System;
using System.Collections.Generic;

namespace BusinessObjects.Models
{
    public partial class SmsOtp : BaseEntity
    {
        public DateTime? CreateAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string? OtpValue { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
