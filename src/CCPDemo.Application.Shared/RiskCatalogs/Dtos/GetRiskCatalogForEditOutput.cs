using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCatalogs.Dtos
{
    public class GetRiskCatalogForEditOutput
    {
        public CreateOrEditRiskCatalogDto RiskCatalog { get; set; }

    }
}