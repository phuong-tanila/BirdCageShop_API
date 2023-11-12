using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class CageDAO
    {
        private BirdCageShopContext _context { get; set; }

        public CageDAO(BirdCageShopContext context)
        {
            _context = context;
        }

        public async Task<List<Cage>> GetCagesAsync()
        {
            return await _context.Cages
                .Include(c => c.CageComponents)
                .ThenInclude(cc => cc.Component)
                .Include(c => c.Images)
                .ToListAsync();
        }
        
        public async Task<Cage> CreateAsync(Cage creatingEntity)
        {
            var createdCage = _context.Cages.AddAsync(creatingEntity).Result.Entity;
            await _context.SaveChangesAsync();
            return createdCage;
        }

        public async Task<List<Cage>> GetNonDeletedCagesAsync()
        {
            return await _context.Cages
                .Where(c => c.IsDeleted == false)
                .Include(c => c.CageComponents)
                .ThenInclude(cc => cc.Component)
                .Include(c => c.Images)
                .ToListAsync();
        }

        public async Task<Cage> GetNonDeletedCagesByIdAsync(Guid key)
        {
            var list = await _context.Cages
                .Include(c => c.CageComponents)
                .ThenInclude(cc => cc.Component)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == key);
            return list;
        }
        public async Task<Cage> GetCageById(Guid id)
        {
            return await _context.Cages.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Cage> UpdateCageAsync(Cage cage)
        {
            _context.Entry(cage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cage;
        }

        public async Task<Cage> DeleteCageAsync(Guid key)
        {
            var cage = await GetNonDeletedCagesByIdAsync(key);
            if (cage != null)
            {
                cage.IsDeleted = true;
                await UpdateCageAsync(cage);
            }
            return cage;
        }

        public async Task<bool> UpdateCageCustomStatusAsync(Guid key, string status, int price, string description)
        {
            try
            {
                var cage = await _context.Cages.AsNoTracking().FirstOrDefaultAsync(c => c.Id == key && c.Status.Contains("PENDING") && c.IsDeleted == false);
                
                if(cage == null) return false;

                var currentCageStatus = cage.Status;
                var customerPhone = currentCageStatus.Split("_")[1];
                cage.Status = status.ToUpper() + "_" + customerPhone;
                cage.Price = price;
                cage.Description = description;
                _context.Cages.Update(cage);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
