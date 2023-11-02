using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _dao;

        public OrderRepository(OrderDAO dao) { _dao = dao; }
        public async Task<List<Order>> GetAllAsync()
            => await _dao.GetAllAsync();

        public async Task<Order?> GetAsync(Guid id)
            => await _dao.GetAsync(id);

        public async Task<List<Order>> GetAllByCustomerAsync(Guid cusId)
            => await _dao.GetAllByCustomerAsync(cusId);

        public async Task AddAsync(Order model)
            => await _dao.AddAsync(model);

        public async Task DeliveringAsync(Order model)
            => await _dao.DeliveringAsync(model);

        public async Task CompleteAsync(Order model)
            => await _dao.CompleteAsync(model);

        public async Task CancelAsync(Order model)
            => await _dao.CancelAsync(model);
    }
}
