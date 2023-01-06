using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ThreatCatalogs.Dtos
{
    public class CreateOrEditThreatCatalogDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ThreatCatalogConsts.MaxNumberLength, MinimumLength = ThreatCatalogConsts.MinNumberLength)]
        public string Number { get; set; }

        public int Grouping { get; set; }

        [Required]
        [StringLength(ThreatCatalogConsts.MaxNameLength, MinimumLength = ThreatCatalogConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int Order { get; set; }

    }
}