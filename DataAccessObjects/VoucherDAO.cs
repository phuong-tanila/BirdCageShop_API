using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class VoucherDAO
    {
        private readonly BirdCageShopContext _context;
        public VoucherDAO(BirdCageShopContext context) { _context = context; }

        public async Task<List<Voucher>> GetAllAsync()
            => await _context.Vouchers.Where(e => e.IsDeleted == false )
                .ToListAsync();

        public async Task<Voucher?> GetByIdAsync(Guid id)
            => await _context.Vouchers.FirstOrDefaultAsync(e => e.Id == id &&
                e.IsDeleted == false &&
                e.ExpirationDate >= DateTime.Now &&
                e.EffectiveDate <= DateTime.Now);

        public async Task<List<Voucher>> GetByCustomerAsync(Customer model)
        {
            var vouchers = await _context.Vouchers.Where(e =>
                e.IsDeleted == false &&
                e.ConditionPoint <= model.Point &&
                e.ExpirationDate >= DateTime.Now &&
                e.EffectiveDate <= DateTime.Now).ToListAsync();
            var orders = await _context.Orders.Where(e => e.CustomerId == model.Id).ToListAsync();
            var result = vouchers.Where(e => !orders.Any(x => x.VoucherId == e.Id)).ToList();
            return result;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            bool isExist = await _context.Orders.AnyAsync(e => e.VoucherId == id &&
                e.Status != (int)OrderStatus.Canceled);
            if (isExist) return false;

            return await _context.Vouchers.AnyAsync(e => e.Id == id &&
                e.IsDeleted == false && e.ExpirationDate > DateTime.Now);
        }

        public async Task AddAsync(Voucher model)
        {
            model.Id = Guid.NewGuid();
            model.IsDeleted = false;
            _context.Vouchers.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Voucher model)
        {
            model.IsDeleted = false;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Voucher model)
        {
            model.IsDeleted = true;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
            => await _context.Vouchers.AnyAsync(e => e.Id == id && e.IsDeleted == false);

    }
}
