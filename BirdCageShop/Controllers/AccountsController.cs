using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

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

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInDTO signInModel)
        {
            try
            {
                var result = await _repo.SignInAsync(signInModel);
                if (result is null)
                {
                    return Unauthorized();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Sign in fail");
            }
        }

        [HttpPost("sign-up")]
        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
        {
            return await _repo.SignUpAsync(model);
        }

        [Authorize]
        [HttpGet("hello")]
        public IActionResult SayHello()
        {
            var role = GetCurrentUser();
            return Ok($"Hi , you are an {role}");
        }

        [Authorize]
        [HttpPost("sign-out")]
        public async Task<IActionResult> Revoke()
        {
            string accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", ""); ;
            if (accessToken is null)
            {
                return BadRequest("Invalid client request");
            }
            ClaimsPrincipal principal = _repo.GetPrincipalFromExpiredToken(accessToken)!;
            if (principal is null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            try
            {
                string username = principal.Identity!.Name!;
                var user = await _repo.FindByNameAsync(username);
                if (user is null) return BadRequest("Invalid user name");
                await _repo.SignOutAsync(user);
            }
            catch (Exception)
            {
                return BadRequest("Something wrong");
            }

            return NoContent();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(Token token)
        {
            if (token is null || token.AccessToken is null || token.RefreshToken is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;

            var principal = _repo.GetPrincipalFromExpiredToken(accessToken);
            if (principal is null || principal.Identity is null || principal.Identity.Name is null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            string username = principal.Identity.Name;
            var user = await _repo.FindByNameAsync(username);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            try
            {
                var newToken = await _repo.GenerateTokenAsync(user);
                return Ok(newToken);
            }
            catch (Exception)
            {
                return BadRequest("Some thing wrong");
            }
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
