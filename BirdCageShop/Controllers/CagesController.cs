using Microsoft.AspNetCore.Mvc;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;
using Repositories;
using DataTransferObjects.CageDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BirdCageShop.Controllers
{

    public class CagesController : ODataController
    {
        private readonly BirdCageShopContext _context;
        private readonly ICageRepository _cageRepository;
        private readonly UserManager<Account> _userManager;
        public CagesController(BirdCageShopContext context, ICageRepository cageRepository, UserManager<Account> userManager)
        {
            _context = context;
            _cageRepository = cageRepository;
            _userManager = userManager;
        }

        // GET: api/Cages
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Cage>>> GetCages()
        {

            return await _cageRepository.GetNonDeletedCagesAsync();
        }
        [HttpPost("odata/[controller]/all")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<Cage>>> GetAllCages()
        {

            return await _cageRepository.GetCagesAsync();
        }
        [HttpPatch("odata/[controller]/status")]
        public async Task<ActionResult> UpdateCustomCageStatus([FromBody] UpdateCageCustomStatusModel model)
        {
            return await _cageRepository.UpdateCageCustomStatusAsync(model) ? NoContent() : BadRequest("Update status fail");
        }
        [EnableQuery]
        // GET: api/Cages/5
        public async Task<ActionResult<Cage>> Get(Guid key)
        {
            var cage = await _cageRepository.GetNonDeletedCageByIdAsync(key);
            if (cage == null)
            {
                return NotFound();
            }
            return cage;
        }
        
        // PUT: api/Cages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{key}")]
        public async Task<IActionResult> Put([FromRoute] Guid key, [FromBody] UpdateCageModel cage)
        {
            if (key != cage.Id)
            {
                return BadRequest();
            }
            var updatedCage = await _cageRepository.UpdateCageAsync(key, cage);
            if (updatedCage is null) return NotFound();
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
            foreach (var item in createdCage.CageComponents)
            {
                item.Cage = null;
            }
            return CreatedAtAction("Get", new { key = createdCage.Id }, createdCage);
        }
        [HttpPost("odata/[controller]/custom")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateCustomCage([FromBody] Cage model)
        {
            var userPhone = HttpContext.User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdCage = await _cageRepository.CreateCustomAsync(model, userPhone);
            //createdCage.CageComponents
            return Ok(createdCage);
        }

        // DELETE: api/Cages/5
        [EnableQuery]
        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(Guid key)
        {
            var cage = await _cageRepository.DeleteCageAsync(key);
            if(cage is null) return NotFound();
            return NoContent();
        }

    }
}
