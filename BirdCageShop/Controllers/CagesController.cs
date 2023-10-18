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
using Repositories;
using DataTransferObjects.CageDTOs;

namespace BirdCageShop.Controllers
{
    
    public class CagesController : ODataController
    {
        private readonly BirdCageShopContext _context;
        private readonly ICageRepository _cageRepository;
        public CagesController(BirdCageShopContext context, ICageRepository cageRepository)
        {
            _context = context;
            _cageRepository = cageRepository;
            
        }

        // GET: api/Cages
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Cage>>> GetCages()
        {
            
            return await _cageRepository.GetCagesAsync();
        }
        [EnableQuery]
        // GET: api/Cages/5
        public async Task<ActionResult<Cage>> Get(Guid key)
        {
            if (_context.Cages == null)
            {
                return NotFound();
            }
            var cage = await _context.Cages.FindAsync(key);

            if (cage == null)
            {
                return NotFound();
            }

            return cage;
        }

        // PUT: api/Cages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{key}")]
        public async Task<IActionResult> Put(Guid key, Cage cage)
        {
            if (key != cage.Id)
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
                if (!CageExists(key))
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
        [EnableQuery]
        [HttpPost]
        public async Task<ActionResult<Cage>> Post([FromBody]CreateCageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdCage = await _cageRepository.CreateAsync(model);

            return CreatedAtAction("GetCage", new { key = createdCage.Id }, createdCage);
        }

        // DELETE: api/Cages/5
        [EnableQuery]
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(Guid key)
        {
            if (_context.Cages == null)
            {
                return NotFound();
            }
            var cage = await _context.Cages.FindAsync(key);
            if (cage == null)
            {
                return NotFound();
            }

            _context.Cages.Remove(cage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CageExists(Guid key)
        {
            return (_context.Cages?.Any(e => e.Id == key)).GetValueOrDefault();
        }
    }
}
