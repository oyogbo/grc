using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskCatalogs
{
    [Table("RiskCatalogs")]
    public class RiskCatalog : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(RiskCatalogConsts.MaxNumberLength, MinimumLength = RiskCatalogConsts.MinNumberLength)]
        public virtual string Number { get; set; }

        public virtual int Grouping { get; set; }

        [Required]
        [StringLength(RiskCatalogConsts.MaxNameLength, MinimumLength = RiskCatalogConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual int Function { get; set; }

        public virtual int Order { get; set; }

    }
}