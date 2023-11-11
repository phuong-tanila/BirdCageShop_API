using BusinessObjects.Models;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFeedbackRepository
    {
        Task<List<OrderDetail>> GetAllAsync();
        Task<List<OrderDetail>> GetByCageAsync(Guid cageId);
        Task<OrderDetail?> GetAsync(Guid id);
        Task AddAsync(OrderDetail model);
    }
}
