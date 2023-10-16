using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BirdCageShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly BirdCageShopContext _context;

        public VouchersController(BirdCageShopContext context)
        {
            _context = context;
        }

        // GET: api/Vouchers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetVouchers()
        {
          if (_context.Vouchers == null)
          {
              return NotFound();
          }
            return await _context.Vouchers.ToListAsync();
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Voucher>> GetVoucher(Guid id)
        {
          //if (_context.Vouchers == null)
          //{
          //    return NotFound();
          //}
            var voucher = await _context.Vouchers.FindAsync(id);

            //if (voucher == null)
            //{
            //    return NotFound();
            //}

            return voucher;
        }

        [HttpGet("1/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Voucher>> GetVoucher1(Guid id)
        {
            if (_context.Vouchers == null)
            {
                return NotFound();
            }
            var voucher = await _context.Vouchers.FindAsync(id);

            if (voucher == null)
            {
                return NotFound();
            }

            return voucher;
        }

        // PUT: api/Vouchers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoucher(Guid id, Voucher voucher)
        {
            if (id != voucher.Id)
            {
                return BadRequest();
            }

            _context.Entry(voucher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoucherExists(id))
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Voucher>> PostVoucher(Voucher voucher)
        {
          if (_context.Vouchers == null)
          {
              return Problem("Entity set 'BirdCageShopContext.Vouchers'  is null.");
          }
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoucher", new { id = voucher.Id }, voucher);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVoucher(Guid id)
        {
            if (_context.Vouchers == null)
            {
                return NotFound();
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoucherExists(Guid id)
        {
            return (_context.Vouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
