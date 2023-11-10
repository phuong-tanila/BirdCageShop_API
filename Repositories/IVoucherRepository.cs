using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IVoucherRepository : IBaseRepository<Voucher>
    {
        Task<bool> IsValidAsync(Guid id);
    }
}
