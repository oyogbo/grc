using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ThreatCatalogs.Dtos
{
    public class GetThreatCatalogForEditOutput
    {
        public CreateOrEditThreatCatalogDto ThreatCatalog { get; set; }

    }
}