using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.SmsOtpDTOs
{
    public class VerifyOtpDTO
    {
        public string PhoneNumber { get; set; }
        public string OtpValue { get; set; }
    }
}
