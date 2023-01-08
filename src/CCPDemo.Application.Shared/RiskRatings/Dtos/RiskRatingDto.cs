using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskRatings.Dtos
{
    public class RiskRatingDto : EntityDto
    {
        public string Name { get; set; }

        public string Color { get; set; }

    }
}