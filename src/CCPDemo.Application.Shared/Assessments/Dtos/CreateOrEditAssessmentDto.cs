using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Assessments.Dtos
{
    public class CreateOrEditAssessmentDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AssessmentConsts.MaxNameLength, MinimumLength = AssessmentConsts.MinNameLength)]
        public string Name { get; set; }

    }
}