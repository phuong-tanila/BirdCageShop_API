using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class FeedbackDAO
    {
        private readonly BirdCageShopContext _context;
        public FeedbackDAO(BirdCageShopContext context) { _context = context; }

        public async Task<List<OrderDetail>> GetAllAsync()
            => await _context.OrderDetails
                .Where(e => e.IsDeleted == false)
                .Include(e => e.Order!.Customer).ToListAsync();

        public async Task<List<OrderDetail>> GetByCageAsync(Guid cageId)
            => await _context.OrderDetails
                .Where(e => e.CageId == cageId && e.Order!.Customer!.Account!.Status != 0
                    && e.IsDeleted == false)
                .Include(e => e.Order!.Customer).ToListAsync();

        public async Task<OrderDetail?> GetAsync(Guid id)
            => await _context.OrderDetails
                .FirstOrDefaultAsync(e => e.Id == id && e.Order!.Customer!.Account!.Status != 0
                    && e.IsDeleted == false);

        public async Task AddAsync(OrderDetail model)
        {
            model.IsDeleted = false;
            model.PostDate = DateTime.Now;
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
