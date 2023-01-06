using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ReviewLevels.Dtos
{
    public class GetReviewLevelForEditOutput
    {
        public CreateOrEditReviewLevelDto ReviewLevel { get; set; }

    }
}