using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Regulations
{
    [Table("regulation")]
    public class Regulation : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int value { get; set; }

        [Required]
        [StringLength(RegulationConsts.MaxnameLength, MinimumLength = RegulationConsts.MinnameLength)]
        public virtual string name { get; set; }

    }
}