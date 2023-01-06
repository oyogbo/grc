using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskTypes.Dtos
{
    public class GetAllRiskTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}