using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Deltas;

namespace BirdCageShop.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly ICustomerRepository _repo;
        private readonly IAccountRepository _accountRepo;

        public CustomersController(ICustomerRepository repo, IAccountRepository accountRepo)
        {
            _repo = repo;
            _accountRepo = accountRepo;
        }

        // GET: odata/Customers
        [EnableQuery]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAsync()
        {
            return Ok(await _repo.GetAllAsync());
        }

        // GET: odata/Customers/5
        [EnableQuery]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<Customer>> GetAsync(Guid? key)
        {
            var model = await _repo.GetByIdAsync((Guid)key!);
            if (model == null) return NotFound();

            return Ok(model);
        }

        // PUT: odata/Customers/5
        [EnableQuery]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> PutAsync(Guid key, [FromBody] Customer model)
        {
            if (!ModelState.IsValid || model is null || key != model.Id)
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

        //// POST: odata/Customers
        //[EnableQuery]
        //[Authorize(Roles = "Ad")]
        //public async Task<ActionResult<Customer>> PostAsync([FromBody] Customer model)
        //{
        //    if (!ModelState.IsValid || model is null)
        //    {
        //        return BadRequest("Invalid format");
        //    }
        //    try
        //    {
        //        await _repo.AddAsync(model);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Create fail");
        //    }

        //    return Created(model);
        //}

        // DELETE: odata/Customers/5
        [EnableQuery]
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

        [EnableQuery]
        public async Task<IActionResult> PatchAsync(Guid key, [FromBody] Delta<Customer> delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await _repo.GetByIdAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            delta.Patch(entity);
            try
            {
                await _repo.SaveChangAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repo.ExistAsync(key))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }
            return Updated(entity);
        }
    }
}
