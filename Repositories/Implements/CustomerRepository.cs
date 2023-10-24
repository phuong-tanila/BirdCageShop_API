using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDAO _dao;

        public CustomerRepository(CustomerDAO dao) { _dao = dao; }
        public async Task<List<Customer>> GetAllAsync()
            => await _dao.GetAllAsync();

        public async Task<Customer?> GetByIdAsync(Guid id)
            => await _dao.GetByIdAsync(id);

        public async Task AddAsync(Customer model)
            => await _dao.AddAsync(model);
        public async Task UpdateAsync(Customer model)
            => await UpdateAsync(model);

        public async Task DeleteAsync(Customer model)
            => await _dao.DeleteAsync(model);

        public async Task<bool> ExistAsync(Guid id)
            => await _dao.ExistAsync(id);

        public async Task<Customer?> GetByAccountIdAsync(string id)
            => await _dao.GetByAccountIdAsync(id);
    }
}
