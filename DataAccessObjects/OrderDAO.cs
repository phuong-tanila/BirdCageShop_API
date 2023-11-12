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
    public class OrderDAO
    {
        private readonly BirdCageShopContext _context;
        public OrderDAO(BirdCageShopContext context) { _context = context; }

        public async Task<List<Order>> GetAllAsync()
            => await _context.Orders.Where(e => e.IsDeleted == false)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Cage)
                .Include(e => e.Voucher)
                .Include(e => e.Customer)
                .ToListAsync();

        public async Task<Order?> GetAsync(Guid id)
            => await _context.Orders.Where(e => e.Id == id && e.IsDeleted == false)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Cage)
                .Include(e => e.Voucher)
                .Include(e => e.Customer)
                .FirstOrDefaultAsync();

        public async Task<List<Order>> GetAllByCustomerAsync(Guid cusId)
           => await _context.Orders.Where(e => e.IsDeleted == false)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Cage)
                .Include(e => e.Voucher)
                .Include(e => e.Customer)
                .ToListAsync();

        public async Task AddAsync(Order model)
        {
            model.Id = Guid.NewGuid();
            model.Status = (int)OrderStatus.Processing;
            model.OrderDate = DateTime.Now;
            int total = 0;
            foreach (var i in model.OrderDetails)
            {
                i.Id = Guid.NewGuid();
                Cage? cage = await _context.Cages.FindAsync(i.CageId);
                i.Price = (int)cage!.Price!;
                total += (int)cage!.Price! * i.Quantity;
            }

            if (model.Description is null) model.Description = string.Empty;

            model.Total = total + model.ShipFee;
            model.IsDeleted = false;

            if (model.VoucherId != null)
            {
                Voucher? voucher = await _context.Vouchers.FindAsync(model.VoucherId!);
                if (voucher != null && voucher!.Discount != 0)
                {
                    model.Total = (int)Math.Ceiling(total - (total * voucher!.Discount));
                }
            }

            _context.Orders.Add(model);

            // Update cage
            foreach (var i in model.OrderDetails)
            {
                var cage = await _context.Cages.FindAsync(i.CageId);
                cage!.InStock = cage.InStock - i.Quantity;
                cage!.Status = "Completed";
                _context.Entry(cage).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeliveringAsync(Order model)
        {
            model.Status = (int)OrderStatus.Delivering;

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CompleteAsync(Order model)
        {
            model.Status = (int)OrderStatus.Completed;
            model.DeliveryDate = DateTime.Now;

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(Order model)
        {
            model.Status = (int)OrderStatus.Canceled;

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
