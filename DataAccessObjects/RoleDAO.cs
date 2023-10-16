using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class RoleDAO
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleDAO(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var newRole = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(newRole);
        }
    }
}
