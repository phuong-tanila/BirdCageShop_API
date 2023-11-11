using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects;
using DataTransferObjects.CageComponentDTOs;
using DataTransferObjects.CageDTOs;
using DataTransferObjects.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Base;

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

            CreateMap<OrderDetail, FeedbackDTO>()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Order!.Customer!.LastName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Order!.Customer!.FirstName))
                .ReverseMap();
        }
    }
}
