using BusinessObjects.Models;
using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpDTO model);
        Task<IdentityResult> SignUpAccountAsync(SignUpAccountDTO model);
        Task<Token> SignInAsync(SignInDTO model);
        Task SignOutAsync(Account model);
        Task<Account> FindByNameAsync(string name);
        Task<Account> FindByIdAsync(string id);
        Task<IdentityResult> UpdateAsync(Account model);
        Task<Token> GenerateTokenAsync(Account model);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken);
    }
}
