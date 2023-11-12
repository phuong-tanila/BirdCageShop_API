using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace BirdCageShop.Controllers
{
    [Route("odata/[Controller]")]
    public class AccountsController : ODataController
    {
        private readonly IAccountRepository _repo;
        private readonly ICustomerRepository _cusRepo;

        public AccountsController(IAccountRepository repo
            , ICustomerRepository cusRepo)
        {
            _repo = repo;
            _cusRepo = cusRepo;
        }

        //[ODataRouteComponent]
        //[ODataRouting]
        [EnableQuery]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInModel)
        {
            try
            {
                var result = await _repo.SignInAsync(signInModel);
                if (result.Succeeded)
                {
                    var user = await _repo.FindByNameAsync(signInModel.Phone);
                    if (user.Status == 0) return BadRequest("The account is locked");

                    //bool isConfirmed = await _repo.IsPhoneNumberConfirmedAsync(user);
                    //if (isConfirmed)
                    //{
                        var token = await _repo.GenerateTokenAsync(user);
                        return Ok(token);
                    //}
                    //return Forbid();
                }
                return NotFound("Phone or password not correct");
            }
            catch (Exception)
            {
                return BadRequest("Sign in fail");
            }
        }

        [HttpPost("sign-up")]
        public async Task<IdentityResult> SignUpAsync([FromBody] SignUpDTO model)
        {
            return await _repo.SignUpAsync(model);
        }

        // POST
        [HttpPost("sign-up-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] SignUpAccountDTO model)
        {
            string role = model.Role;
            if (role != "Customer" && role != "Staff" && role != "Manager")
            {
                return BadRequest("Invalid format");
            }
            try
            {
                var result = await _repo.SignUpAccountAsync(model);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception)
            {
                return BadRequest("Some thing wrong");
            }
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
            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", ""); ;
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
        public async Task<IActionResult> RefreshToken([FromBody] Token token)
        {
            if (token is null || token.AccessToken is null || token.RefreshToken is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = token.AccessToken;
            string refreshToken = token.RefreshToken;
            try
            {
                // Check valid token
                var principal = _repo.GetPrincipalFromExpiredToken(accessToken);
                if (principal is null || principal.Identity is null || principal.Identity.Name is null)
                {
                    return BadRequest("Invalid access token");
                }
                string username = principal.Identity.Name;
                var user = await _repo.FindByNameAsync(username);
                if (user is null || user.RefreshToken != refreshToken
                        || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid refresh token");
                }

                // Check expDate
                var expClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                if (expClaim is null)
                {
                    return BadRequest("Invalid access token");
                }
                var utcExpireDate = long.Parse(expClaim.Value);
                var expireDate = DateTimeOffset.FromUnixTimeSeconds(utcExpireDate).UtcDateTime;
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok("Access token has not yet expired");
                }

                // Create new token
                var newToken = await _repo.GenerateTokenAsync(user);
                return Ok(newToken);
            }
            catch (Exception)
            {
                return BadRequest("Invalid access token or refresh token");
            }
        }

        // GET: odata/Accounts/user-profile
        [HttpGet("user-profile")]
        [Authorize]
        public async Task<ActionResult<Customer>> GetProfileAsync()
        {
            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "");
            if (accessToken == null) return BadRequest("Invalid access token");

            try
            {
                var user = await _repo.FindByTokenAsync(accessToken);
                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest("Something wrong");
            }
        }

        // PUT: odata/Accounts/edit-profile
        [HttpPut("edit-profile")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> UpdateProfileAsync([FromBody] Customer model)
        {
            if (!ModelState.IsValid || model is null)
            {
                return BadRequest("Invalid format");
            }

            string accessToken = Request.Headers[HeaderNames.Authorization]
                .ToString().Replace("Bearer ", "");
            if (accessToken == null) return BadRequest("Invalid access token");

            try
            {
                var user = await _repo.FindByTokenAsync(accessToken);
                if (user == null) return NotFound();

                var customer = await _cusRepo.GetByAccountIdAsync(user.Id);
                if (customer is null) return NotFound();

                if (customer.Id != model.Id) return BadRequest("Invalid format");

                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Address = model.Address;

                await _cusRepo.UpdateAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: odata/Accounts/edit-profile
        [HttpPut("lock-account")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> LockAccountAsync(string key)
        {
            try
            {
                var account = await _repo.FindByIdAsync(key);
                if (account is null) return NotFound();
                account.Status = 0;
                var result = await _repo.UpdateAsync(account);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //public async Task<ActionResult<Account>> GetAccountFromToken()
        //{
        //    string accessToken = Request.Headers[HeaderNames.Authorization]
        //        .ToString().Replace("Bearer ", "");
        //    //if (accessToken is null) return BadRequest("Invalid client request");

        //    try
        //    {
        //        ClaimsPrincipal principal = _repo.GetPrincipalFromExpiredToken(accessToken)!;
        //        if (principal is null) return BadRequest("Invalid access token");

        //        string username = principal.Identity!.Name!;
        //        var user = await _repo.FindByNameAsync(username);
        //        if (user is null) return BadRequest("Invalid user");

        //        return Ok(user);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Invalid access token");
        //    }
        //}

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
