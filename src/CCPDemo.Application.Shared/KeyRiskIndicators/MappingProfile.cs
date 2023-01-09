using AutoMapper;
using CCPDemo.KeyRiskIndicators.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CCPDemo.KeyRiskIndicators
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateOrEditKeyRiskIndicatorDto, RCSAModel>().ReverseMap();
        }
    }
}
