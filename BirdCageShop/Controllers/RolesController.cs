using DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace BirdCageShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repo;

        public RolesController(IRoleRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IdentityResult> PostRoleAsync(string name)
        {
            return await _repo.CreateRoleAsync(name);
        }
    }
}
