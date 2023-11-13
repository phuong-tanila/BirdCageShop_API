using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects;
using DataTransferObjects.AccountDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class AccountDAO
    {
        private readonly BirdCageShopContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountDAO(BirdCageShopContext context, UserManager<Account> userManager
            , SignInManager<Account> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //public async Task<Token> SignInAsync(SignInDTO model)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(model.Phone, model.Password, false, false);

        //    if (!result.Succeeded)
        //    {
        //        return null!;
        //    }
        //    var user = await _userManager.FindByNameAsync(model.Phone);

        //    var token = await GenerateTokenAsync(user);

        //    return token;
        //}


        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
        {
            var user = new Account
            {
                PhoneNumber = model.Phone,
                UserName = model.Phone,
                Status = 1
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new List<string>() { "Customer" });
                await _context.Customers.AddAsync(new Customer { AccountId = user.Id , Point = 0});
                await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<IdentityResult> SignUpAccountAsync(SignUpAccountDTO model)
        {
            var user = new Account
            {
                PhoneNumber = model.Phone,
                UserName = model.Phone,
                Status = 1,
                PhoneNumberConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new List<string>() { model.Role });
                await _context.Customers.AddAsync(new Customer { AccountId = user.Id });
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task SignOutAsync(Account account)
        {
            account.RefreshToken = null;
            await _userManager.UpdateAsync(account);
        }

        //        public async Task<Token?> RefreshToken(Token token)
        //        {
        //            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
        //            if (principal == null)
        //            {
        //                return null;
        //            }
        //#pragma warning disable CS8602 // Dereference of a possibly null reference.
        //#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        //            string username = principal.Identity.Name;
        //#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        //#pragma warning restore CS8602 // Dereference of a possibly null reference.

        //            var user = await _userManager.FindByNameAsync(username);

        //            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        //            {
        //                return null;
        //            }

        //            var newToken = await GenerateTokenAsync(user);

        //            return newToken;
        //        }

        public async Task<Token> GenerateTokenAsync(Account user)
        {
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var accessToken = new JwtSecurityTokenHandler().WriteToken(GenerateAccessToken(authClaims));
            var refreshToken = GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);

            return new Token { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private JwtSecurityToken GenerateAccessToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters
                , out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken 
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256
                , StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public async Task<Account> FindByUserNameAsync(string username)
        {
            return await _context.Users.Include(u => u.Customers).FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
