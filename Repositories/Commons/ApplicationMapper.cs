using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects.CageComponentDTOs;
using DataTransferObjects.CageDTOs;
using DataTransferObjects.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Commons
{
    public class ApplicationMapper : Profile
    {

        public ApplicationMapper()
        {
            createCageModels();

        }

        private void createCageModels()
        {
            CreateMap<CreateCageModel, Cage>();
            CreateMap<CreateCustomCageModel, Cage>();

            CreateMap<UpdateCageModel, Cage>();
            CreateMap<CreateImageModel, Image>();
            CreateMap<CreateCageComponentModel, CageComponent>();

        }
    }
}
