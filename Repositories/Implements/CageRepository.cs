using AutoMapper;
using BusinessObjects.Models;
using DataAccessObjects;
using DataTransferObjects.CageDTOs;
using Repositories.Commons.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class CageRepository : ICageRepository
    {
        private readonly CageDAO _cageDAO;

        private readonly IComponentRepository _componentRepository;

        private readonly IMapper _mapper;
        public CageRepository(
            CageDAO cageDAO,
            IMapper mapper,
            IComponentRepository componentRepository
        )
        {
            _cageDAO = cageDAO;
            _mapper = mapper;
            _componentRepository = componentRepository;
        }

        public async Task<List<Cage>> GetNonDeletedCagesAsync()
        {
            return await _cageDAO.GetNonDeletedCagesAsync();
        }
        public async Task<List<Cage>> GetCagesAsync()
        {
            return await _cageDAO.GetCagesAsync();
        }
        public async Task<Cage> CreateAsync(CreateCageModel model)
        {
            var creatingEntity = _mapper.Map<Cage>(model);
            creatingEntity.Id = Guid.NewGuid();
            creatingEntity.Rating = 0;
            creatingEntity.CreateDate = DateTime.Now;
            List<string> errorMessages = new List<string>();
            List<string> componentTypes = ComponentTypes.GetComponentTypes();
            foreach (var item in creatingEntity.CageComponents)
            {
                var component = await _componentRepository.GetByIdAsync(item.ComponentId.Value);
                if (component == null)
                {
                    errorMessages.Add($"Component with id: {item.ComponentId} not found or has been deleted!!");
                }
                else
                {
                    if (componentTypes.Contains(component.Type))
                    {
                        componentTypes.Remove(component.Type);
                    }
                }
                    
            }
            componentTypes.ForEach(ct =>
            {
                errorMessages.Add($"The cage is missing {ct} component");
            });
            if(errorMessages.Count > 0 )  
                throw new InvalidCageComponentException(errorMessages.ToArray()); 
            //creatingEntity.
            return await _cageDAO.CreateAsync(creatingEntity);
        }
        

        public async Task<Cage> GetNonDeletedCageByIdAsync(Guid key)
        {
            return await _cageDAO.GetNonDeletedCagesByIdAsync(key);
        }

        public async Task<Cage> UpdateCageAsync(Guid key, UpdateCageModel model)
        {
            var mappedCage = _mapper.Map<Cage>(model);
            var cageInDb = await _cageDAO.GetCageById(mappedCage.Id);
            if (cageInDb == null)
            {
                return null;
            }
            mappedCage.CreateDate = cageInDb.CreateDate;
            mappedCage.Rating = cageInDb.Rating;
            //mappedCage.
            return await _cageDAO.UpdateCageAsync(mappedCage);
        }

        public async Task<Cage> DeleteCageAsync(Guid key)
        {
            return await _cageDAO.DeleteCageAsync(key);
        }
    }
}
