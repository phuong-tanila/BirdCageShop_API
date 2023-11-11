using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories;
using Microsoft.AspNetCore.Authorization;

namespace BirdCageShop.Controllers
{
    public class ComponentsController : ODataController
    {
        private readonly IComponentRepository _repo;

        public ComponentsController(IComponentRepository repo)
        {
            _repo = repo;
        }

        // GET: odata/Components
        [EnableQuery]
        //[Authorize(Roles = "Staff")]
        public async Task<ActionResult<IEnumerable<Component>>> GetAsync()
        {
            return Ok(await _repo.GetAllAsync());
        }

        // GET: odata/Components/5
        [EnableQuery]
        //[Authorize(Roles = "Staff, Customer")]
        public async Task<ActionResult<Component>> GetAsync(Guid key)
        {
            var model = await _repo.GetByIdAsync(key);

            if (model == null) return NotFound();

            return Ok(model);
        }

        // PUT: odata/Components/5
        [EnableQuery]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> PutAsync(Guid key, [FromBody] Component model)
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

        // POST: odata/Components
        [EnableQuery]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult<Component>> PostAsync([FromBody] Component model)
        {
            if (!ModelState.IsValid || model is null)
            {
                return BadRequest("Invalid format");
            }
            try
            {
                await _repo.AddAsync(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Created(model);
        }

        // DELETE: odata/Components/5
        [EnableQuery]
        [Authorize(Roles = "Staff")]
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
    }
}
