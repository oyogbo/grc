using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskLevels
{
    [Table("RiskLevels")]
    public class RiskLevel : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal Value { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxNameLength, MinimumLength = RiskLevelConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxColorLength, MinimumLength = RiskLevelConsts.MinColorLength)]
        public virtual string Color { get; set; }

        [Required]
        [StringLength(RiskLevelConsts.MaxDisplayNameLength, MinimumLength = RiskLevelConsts.MinDisplayNameLength)]
        public virtual string DisplayName { get; set; }

    }
}