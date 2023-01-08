using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskStatuses.Dtos
{
    public class GetAllRiskStatusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}