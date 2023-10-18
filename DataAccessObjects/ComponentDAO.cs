using BusinessObjects.Models;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class ComponentDAO
    {
        private readonly BirdCageShopContext _context;
        public ComponentDAO(BirdCageShopContext context) { _context = context; }

        public async Task<List<Component>> GetAllAsync()
            => await _context.Components.Where(e => e.IsDeleted == false).ToListAsync();

        public async Task<Component?> GetByIdAsync(Guid id)
            => await _context.Components.FirstOrDefaultAsync(e => e.Id == id
                && e.IsDeleted == false);

        public async Task AddAsync(Component model)
        {
            model.Id = Guid.NewGuid();
            model.IsDeleted = false;
            _context.Components.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Component model)
        {
            model.IsDeleted = false;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Component model)
        {
            model.IsDeleted = true;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
            => await _context.Components.AnyAsync(e => e.Id == id && e.IsDeleted == false);
    }
}
