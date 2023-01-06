using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.ThreatCatalogs.Dtos
{
    public class ThreatCatalogDto : EntityDto
    {
        public string Number { get; set; }

        public int Grouping { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

    }
}