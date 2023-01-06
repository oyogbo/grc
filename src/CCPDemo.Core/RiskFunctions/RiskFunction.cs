using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskFunctions
{
    [Table("RiskFunctions")]
    public class RiskFunction : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(RiskFunctionConsts.MaxNameLength, MinimumLength = RiskFunctionConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}