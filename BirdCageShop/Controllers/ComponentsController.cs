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
    public class ComponentsController : ODataController
    {
        private readonly BirdCageShopContext _context;

        public ComponentsController(BirdCageShopContext context)
        {
            _context = context;
        }

        // GET: odata/Components
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Component>>> GetAsync()
        {
            if (_context.Components == null)
            {
                return NotFound();
            }
            return await _context.Components.ToListAsync();
        }

        // GET: odata/Components/5
        [EnableQuery]
        public async Task<ActionResult<Component>> GetAsync(Guid key)
        {
            if (_context.Components == null)
            {
                return NotFound();
            }
            var component = await _context.Components.FindAsync(key);

            if (component == null)
            {
                return NotFound();
            }

            return component;
        }

        // PUT: odata/Components/5
        [EnableQuery]
        public async Task<IActionResult> PutAsync(Guid key, Component component)
        {
            if (key != component.Id)
            {
                return BadRequest();
            }

            _context.Entry(component).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComponentExists(key))
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

        // POST: odata/Components
        [EnableQuery]
        public async Task<ActionResult<Component>> PostAsync([FromBody] Component model)
        {
            if (model == null)
            {
                return BadRequest();
            }
             _context.Components.Add(model);
            await _context.SaveChangesAsync();

            return Created(model);
        }

        // DELETE: odata/Components/5
        [EnableQuery]
        public async Task<IActionResult> DeleteAsync(Guid key)
        {
            if (_context.Components == null)
            {
                return NotFound();
            }
            var component = await _context.Components.FindAsync(key);
            if (component == null)
            {
                return NotFound();
            }

            _context.Components.Remove(component);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComponentExists(Guid id)
        {
            return (_context.Components?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
