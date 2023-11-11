using AutoMapper;
using BusinessObjects.Models;
using DataAccessObjects;
using DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackDAO _dao;

        public FeedbackRepository(FeedbackDAO dao)
        {
            _dao = dao;
        }
        public async Task<List<OrderDetail>> GetAllAsync()
            => await _dao.GetAllAsync();
        public async Task<List<OrderDetail>> GetByCageAsync(Guid cageId)
            => await _dao.GetByCageAsync(cageId);

        public async Task<OrderDetail?> GetAsync(Guid id)
            => await _dao.GetAsync(id);

        public async Task AddAsync(OrderDetail model)
            => await _dao.AddAsync(model);
    }
}
