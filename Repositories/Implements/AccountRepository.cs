using DataAccessObjects;
using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private AccountDAO _dao;
        public AccountRepository(AccountDAO dao) { _dao = dao; }

        public async Task<string> SignInAsync(SignInDTO model) => await _dao.SignInAsync(model);

        public async Task<IdentityResult> SignUpAsync(SignUpDTO model) => await _dao.SignUpAsync(model);
    }
}
