using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Security.Claims;

namespace BirdCageShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _repo;

        public AccountsController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDTO signInModel)
        {
            var result = await _repo.SignInAsync(signInModel);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        [HttpPost("SignUp")]
        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
        {
            return await _repo.SignUpAsync(model);
        }

        [HttpGet("Hello")]
        public IActionResult SayHello()
        {
            var role = GetCurrentUser();
            return Ok($"Hi , you are an {role}");
        }
        private string? GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            }
            return null!;
        }
    }
}
