using AutoMapper;
using BusinessObjects.Models;
using DataAccessObjects;
using DataTransferObjects.CageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class CageRepository : ICageRepository
    {
        private CageDAO _cageDAO { get; }
        private IMapper _mapper { get; set; }
        public CageRepository(
            CageDAO cageDAO, 
            IMapper mapper
            )
        {
            _cageDAO = cageDAO;
            _mapper = mapper;
        }

        public async Task<List<Cage>> GetCagesAsync()
        {
            return await _cageDAO.GetCages();
        }

        public async Task<Cage> CreateAsync(CreateCageModel model)
        {
            var creatingEntity = _mapper.Map<Cage>(model);
            creatingEntity.Id = Guid.NewGuid();
            creatingEntity.Rating = 0;
            creatingEntity.CreateDate = DateTime.Now;
            //creatingEntity.
            return await _cageDAO.CreateAsync(creatingEntity);
        }
    }
}
