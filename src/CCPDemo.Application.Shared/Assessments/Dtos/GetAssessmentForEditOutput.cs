using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Assessments.Dtos
{
    public class GetAssessmentForEditOutput
    {
        public CreateOrEditAssessmentDto Assessment { get; set; }

    }
}