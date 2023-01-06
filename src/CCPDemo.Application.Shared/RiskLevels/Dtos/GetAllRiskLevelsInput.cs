using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskLevels.Dtos
{
    public class GetAllRiskLevelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public decimal? MaxValueFilter { get; set; }
        public decimal? MinValueFilter { get; set; }

        public string NameFilter { get; set; }

        public string ColorFilter { get; set; }

        public string DisplayNameFilter { get; set; }

    }
}