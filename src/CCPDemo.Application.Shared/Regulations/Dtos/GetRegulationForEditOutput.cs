using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Regulations.Dtos
{
    public class GetRegulationForEditOutput
    {
        public CreateOrEditRegulationDto Regulation { get; set; }

    }
}