using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.AccountDTO
{
    public class AccountDTO : Account
    {
        public virtual ICollection<string>? Roles { get; set; }
    }
}
