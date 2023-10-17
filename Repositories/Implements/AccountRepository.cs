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
        public AccountRepository(AccountDAO dao, UserManager<Account> userManager)
        {
            _dao = dao;
            _userManager = userManager;
        }

        public async Task<Token> SignInAsync(SignInDTO model)
            => await _dao.SignInAsync(model);
        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
            => await _dao.SignUpAsync(model);
        public Task SignOutAsync(Account model)
            => _dao.SignOutAsync(model);
        public Task<Account> FindByNameAsync(string name)
            => _userManager.FindByNameAsync(name);
        public Task<Token> GenerateTokenAsync(Account model)
            => _dao.GenerateTokenAsync(model);
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken)
            => _dao.GetPrincipalFromExpiredToken(accessToken);

    }
}
