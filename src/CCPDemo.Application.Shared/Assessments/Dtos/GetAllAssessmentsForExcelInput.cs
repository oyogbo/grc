using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Assessments.Dtos
{
    public class GetAllAssessmentsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}