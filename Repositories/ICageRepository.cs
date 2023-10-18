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

        Task<Cage> CreateAsync(CreateCageModel model);
    }
}
