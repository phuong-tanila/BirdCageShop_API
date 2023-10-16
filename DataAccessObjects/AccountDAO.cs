using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public async Task<string> SignInAsync(SignInDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Phone, model.Password, false, false);
            
            if (!result.Succeeded)
            {
                return string.Empty;
            }
            var user = await _userManager.FindByNameAsync(model.Phone);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                //new Claim(ClaimTypes.MobilePhone, model.Phone),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
        {
            var user = new Account
            {
                PhoneNumber = model.Phone,
                UserName = model.Phone
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new List<string>() { "Customer" });
            }
            return result;
        }

    }
}
