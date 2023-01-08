using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskRatings.Dtos
{
    public class GetRiskRatingForEditOutput
    {
        public CreateOrEditRiskRatingDto RiskRating { get; set; }

    }
}