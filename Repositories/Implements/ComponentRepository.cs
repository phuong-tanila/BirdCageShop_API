using BusinessObjects.Models;
using DataAccessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly ComponentDAO _dao;

        public ComponentRepository(ComponentDAO dao) { _dao = dao; }

        public async Task<List<Component>> GetAllAsync()
            => await _dao.GetAllAsync();

        public async Task<Component?> GetByIdAsync(Guid id)
            => await _dao.GetByIdAsync(id);

        public async Task AddAsync(Component model)
            => await _dao.AddAsync(model);

        public async Task UpdateAsync(Component model)
            => await _dao.UpdateAsync(model);

        public async Task DeleteAsync(Component model)
            => await _dao.DeleteAsync(model);

        public Task<bool> ExistAsync(Guid id)
            => _dao.ExistAsync(id);
    }
}
