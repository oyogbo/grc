using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskRatings.Dtos
{
    public class CreateOrEditRiskRatingDto : EntityDto<int?>
    {

        [Required]
        public string Name { get; set; }

        public string Color { get; set; }

    }
}