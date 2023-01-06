using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.ThreatGroupings
{
    [Table("ThreatGrouping")]
    public class ThreatGrouping : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(ThreatGroupingConsts.MaxNameLength, MinimumLength = ThreatGroupingConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual short Default { get; set; }

        public virtual int Order { get; set; }

    }
}