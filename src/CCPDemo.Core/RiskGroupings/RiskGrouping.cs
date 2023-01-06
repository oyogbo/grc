using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskGroupings
{
    [Table("RiskGroupings")]
    public class RiskGrouping : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(RiskGroupingConsts.MaxNameLength, MinimumLength = RiskGroupingConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual short Default { get; set; }

        public virtual int Order { get; set; }

    }
}