using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskCatalogs.Dtos
{
    public class GetAllRiskCatalogsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NumberFilter { get; set; }

        public int? MaxGroupingFilter { get; set; }
        public int? MinGroupingFilter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaxFunctionFilter { get; set; }
        public int? MinFunctionFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

    }
}