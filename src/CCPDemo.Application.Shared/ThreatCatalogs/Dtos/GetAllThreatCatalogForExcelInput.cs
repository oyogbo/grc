using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.ThreatCatalogs.Dtos
{
    public class GetAllThreatCatalogForExcelInput
    {
        public string Filter { get; set; }

        public string NumberFilter { get; set; }

        public int? MaxGroupingFilter { get; set; }
        public int? MinGroupingFilter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

    }
}