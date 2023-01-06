using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCatalogs.Dtos
{
    public class CreateOrEditRiskCatalogDto : EntityDto<int?>
    {

        [Required]
        [StringLength(RiskCatalogConsts.MaxNumberLength, MinimumLength = RiskCatalogConsts.MinNumberLength)]
        public string Number { get; set; }

        public int Grouping { get; set; }

        [Required]
        [StringLength(RiskCatalogConsts.MaxNameLength, MinimumLength = RiskCatalogConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int Function { get; set; }

        public int Order { get; set; }

    }
}