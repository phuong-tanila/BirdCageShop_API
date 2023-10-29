using BusinessObjects.Models;
using DataAccessObjects;
using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDAO _dao;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        public AccountRepository(AccountDAO dao, UserManager<Account> userManager
            , SignInManager<Account> signInManager)
        {
            _dao = dao;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> SignInAsync(SignInDTO model)
            => await _signInManager.
                PasswordSignInAsync(model.Phone, model.Password, false, false);
        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
            => await _dao.SignUpAsync(model);
        public async Task<IdentityResult> SignUpAccountAsync(SignUpAccountDTO model)
            => await _dao.SignUpAccountAsync(model);
        public async Task SignOutAsync(Account model)
            => await _dao.SignOutAsync(model);
        public async Task<bool> IsPhoneNumberConfirmedAsync(Account model)
            => await _userManager.IsPhoneNumberConfirmedAsync(model);
        public async Task<Account> FindByNameAsync(string name)
            => await _userManager.FindByNameAsync(name);
        public async Task<Account> FindByIdAsync(string id)
            => await _userManager.FindByIdAsync(id);
        public async Task<IdentityResult> UpdateAsync(Account model)
            => await _userManager.UpdateAsync(model);
        public async Task<Token> GenerateTokenAsync(Account model)
            => await _dao.GenerateTokenAsync(model);
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken)
            => _dao.GetPrincipalFromExpiredToken(accessToken);

    }
}
