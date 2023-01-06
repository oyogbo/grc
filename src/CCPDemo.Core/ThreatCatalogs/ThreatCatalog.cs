using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.ThreatCatalogs
{
    [Table("ThreatCatalog")]
    public class ThreatCatalog : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(ThreatCatalogConsts.MaxNumberLength, MinimumLength = ThreatCatalogConsts.MinNumberLength)]
        public virtual string Number { get; set; }

        public virtual int Grouping { get; set; }

        [Required]
        [StringLength(ThreatCatalogConsts.MaxNameLength, MinimumLength = ThreatCatalogConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        public virtual string Description { get; set; }

        public virtual int Order { get; set; }

    }
}