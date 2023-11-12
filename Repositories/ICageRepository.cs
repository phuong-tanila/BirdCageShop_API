using BusinessObjects.Models;
using DataTransferObjects.CageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICageRepository
    {
        Task<List<Cage>> GetCagesAsync();
        Task<List<Cage>> GetNonDeletedCagesAsync();
        Task<Cage> CreateAsync(CreateCageModel model);
        Task<Cage> GetNonDeletedCageByIdAsync(Guid key);
        Task<Cage> UpdateCageAsync(Guid key, UpdateCageModel cage);

        Task<bool> UpdateCageCustomStatusAsync(UpdateCageCustomStatusModel model);
        Task<Cage> DeleteCageAsync(Guid key);
        Task<Cage> CreateCustomAsync(Cage model, string userPhone);
    }
}
