using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public enum OrderStatus
    {
        Processing = 1,
        Delivering = 2,
        Completed = 3,
        Canceled = 4
    }
}
