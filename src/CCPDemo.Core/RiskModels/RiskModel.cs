using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskModels
{
    [Table("RiskModels")]
    public class RiskModel : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(RiskModelConsts.MaxNameLength, MinimumLength = RiskModelConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}