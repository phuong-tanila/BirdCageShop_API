using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CustomerDAO
    {
        private readonly BirdCageShopContext _context;
        public CustomerDAO(BirdCageShopContext context) { _context = context; }

        public async Task<List<Customer>> GetAllAsync()
            => await _context.Customers.Where(e => e.IsDeleted == false).ToListAsync();

        public async Task<Customer?> GetByIdAsync(Guid id)
            => await _context.Customers.FirstOrDefaultAsync(e => e.Id == id
                && e.IsDeleted == false);
        public async Task<Customer?> GetByAccountIdAsync(string id)
        {
          var a =  await _context.Customers.FirstOrDefaultAsync(e => e.AccountId == id
                && e.IsDeleted == false);
            return a;
        }

        public async Task AddAsync(Customer model)
        {
            model.Id = Guid.NewGuid();
            model.IsDeleted = false;
            _context.Customers.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer model)
        {
            model.IsDeleted = false;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer model)
        {
            model.IsDeleted = true;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
            => await _context.Customers.AnyAsync(e => e.Id == id && e.IsDeleted == false);
    }
}
