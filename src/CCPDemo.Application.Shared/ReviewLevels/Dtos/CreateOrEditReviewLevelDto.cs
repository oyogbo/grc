using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ReviewLevels.Dtos
{
    public class CreateOrEditReviewLevelDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(ReviewLevelConsts.MaxNameLength, MinimumLength = ReviewLevelConsts.MinNameLength)]
        public string Name { get; set; }

    }
}