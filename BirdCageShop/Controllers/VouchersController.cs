using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<Voucher>>> GetAsync()
        {
            return Ok(await _repo.GetAllAsync());
        }

        // GET: odata/Vouchers/5
        [EnableQuery]
        //[Authorize]
        public async Task<ActionResult<Voucher>> GetAsync(Guid key)
        {
            var model = await _repo.GetByIdAsync(key);

            if (model == null) return NotFound();

            return Ok(model);
        }

        [EnableQuery]
        //[Authorize(Roles = "Staff")]
        public async Task<IActionResult> PutAsync(Guid key, [FromBody] Voucher model)
        {
            if (!ModelState.IsValid || model is null || key != model.Id || !IsValid(model))
            {
                return BadRequest("Invalid format");
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
        //[Authorize(Roles = "Staff")]
        public async Task<ActionResult<Voucher>> PostAsync([FromBody][BindRequired] Voucher model)
        {
            if (!ModelState.IsValid || model is null || !IsValid(model))
            {
                return BadRequest("Invalid format");
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
        //[Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteAsync(Guid key)
        {
            var model = await _repo.GetByIdAsync(key);
            if (model is null)
            {
                return NotFound();
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
