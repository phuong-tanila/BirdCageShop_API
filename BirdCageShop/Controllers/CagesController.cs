using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;

namespace BirdCageShop.Controllers
{
    
    public class CagesController : ODataController
    {
        private readonly BirdCageShopContext _context;

        public CagesController(BirdCageShopContext context)
        {
            _context = context;
        }

        // GET: api/Cages
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Cage>>> GetCages()
        {
          if (_context.Cages == null)
          {
              return NotFound();
          }
            return await _context.Cages.ToListAsync();
        }

        // GET: api/Cages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cage>> GetCage(Guid id)
        {
          if (_context.Cages == null)
          {
              return NotFound();
          }
            var cage = await _context.Cages.FindAsync(id);

            if (cage == null)
            {
                return NotFound();
            }

            return cage;
        }

        // PUT: api/Cages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCage(Guid id, Cage cage)
        {
            if (id != cage.Id)
            {
                return BadRequest();
            }

            _context.Entry(cage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cage>> PostCage(Cage cage)
        {
          if (_context.Cages == null)
          {
              return Problem("Entity set 'BirdCageShopContext.Cages'  is null.");
          }
            _context.Cages.Add(cage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCage", new { id = cage.Id }, cage);
        }

        // DELETE: api/Cages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCage(Guid id)
        {
            if (_context.Cages == null)
            {
                return NotFound();
            }
            var cage = await _context.Cages.FindAsync(id);
            if (cage == null)
            {
                return NotFound();
            }

            _context.Cages.Remove(cage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CageExists(Guid id)
        {
            return (_context.Cages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
