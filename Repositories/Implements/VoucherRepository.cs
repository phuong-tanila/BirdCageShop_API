using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly VoucherDAO _dao;

        public VoucherRepository(VoucherDAO dao)
        {
            _dao = dao;
        }
        public async Task<List<Voucher>> GetAllAsync()
            => await _dao.GetAllAsync();

        public async Task<Voucher?> GetByIdAsync(Guid id)
            => await _dao.GetByIdAsync(id);

        public async Task AddAsync(Voucher model)
            => await _dao.AddAsync(model);

        public async Task UpdateAsync(Voucher model)
            => await _dao.UpdateAsync(model);

        public async Task DeleteAsync(Voucher model)
            => await _dao.DeleteAsync(model);

        public async Task<bool> ExistAsync(Guid id)
            => await _dao.ExistAsync(id);
    }
}
