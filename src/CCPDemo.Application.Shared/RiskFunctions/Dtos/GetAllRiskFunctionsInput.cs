using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskFunctions.Dtos
{
    public class GetAllRiskFunctionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxValueFilter { get; set; }
        public int? MinValueFilter { get; set; }

        public string NameFilter { get; set; }

    }
}