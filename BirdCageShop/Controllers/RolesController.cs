using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories;

namespace BirdCageShop.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RolesController : ODataController
    {
        private readonly IRoleRepository _repo;

        public RolesController(IRoleRepository repo)
        {
            _repo = repo;
        }

        [EnableQuery]
        public async Task<IdentityResult> PostAsync([FromBody] Role model)
        {
            return await _repo.CreateRoleAsync(model.Name!);
        }
    }
}
