using DataAccessObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class RoleRepository : IRoleRepository
    {
        private RoleDAO _dao;
        public RoleRepository(RoleDAO dao) { _dao = dao; }

        public async Task<IdentityResult> CreateRoleAsync(string name) => await _dao.CreateRoleAsync(name);
    }
}
