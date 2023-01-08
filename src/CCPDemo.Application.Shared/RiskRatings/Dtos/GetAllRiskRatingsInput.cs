using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskRatings.Dtos
{
    public class GetAllRiskRatingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ColorFilter { get; set; }

    }
}