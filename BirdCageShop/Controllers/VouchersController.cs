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
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Microsoft.AspNetCore.OData.Query;

namespace BirdCageShop.Controllers
{
    public class VouchersController : ODataController
    {
        private readonly IVoucherRepository _repo;

        public VouchersController(IVoucherRepository repo)
        {
            _repo = repo;
        }

        // GET: odata/Vouchers
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAsync()
        {
            var a = await _repo.GetAllAsync();
            return a;
        }

        // GET: odata/Vouchers/5
        [EnableQuery]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<Voucher>> GetAsync(Guid key)
        {
            var voucher = await _repo.GetByIdAsync(key);

            if (voucher == null) return NotFound();

            return voucher;
        }

        [EnableQuery]
        public async Task<IActionResult> PutAsync(Guid key, [FromBody] Voucher model)
        {
            if (key != model.Id || !IsValid(model))
            {
                return BadRequest();
            }
            var isExist = await _repo.ExistAsync(key);
            if (!isExist)
            {
                return NotFound();
            }
            try
            {
                await _repo.UpdateAsync(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: odata/Vouchers
        [EnableQuery]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Voucher>> PostAsync([FromBody] Voucher model)
        {
            if (model == null || !IsValid(model))
            {
                return BadRequest();
            }
            try
            {
                await _repo.AddAsync(model);
            }
            catch (Exception)
            {
                return BadRequest("Create fail");
            }

            return Created(model);
        }

        // DELETE: odata/Vouchers/5
        [EnableQuery]
        //[Authorize]
        public async Task<IActionResult> DeleteAsync(Guid key)
        {
            var model = await _repo.GetByIdAsync(key);
            if (model is null)
            {
                return NotFound();
            }
            if (!IsValid(model))
            {
                return BadRequest();
            }
            try
            {
                await _repo.DeleteAsync(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        private bool IsValid(Voucher model)
        {
            if (model.ExpirationDate <= model.EffectiveDate) return false;
            if (model.ExpirationDate <= DateTime.Now) return false;

            return true;
        }
    }
}
