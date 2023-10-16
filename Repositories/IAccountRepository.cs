using DataTransferObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUpAsync(SignUpDTO model);
        Task<string> SignInAsync(SignInDTO model);
    }
}
