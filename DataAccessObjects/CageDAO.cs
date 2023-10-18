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
    public class CageDAO
    {
        private BirdCageShopContext  _context { get; set; }

        public CageDAO(BirdCageShopContext context)
        {
            _context = context;
        }

        public async Task<List<Cage>> GetCages()
        {
            return await _context.Cages.ToListAsync();
        }

        public async Task<Cage> CreateAsync(Cage creatingEntity)
        {
            var createdCage =  _context.Cages.AddAsync(creatingEntity).Result.Entity;   
            await _context.SaveChangesAsync();
            return createdCage;
        }
    }
}
