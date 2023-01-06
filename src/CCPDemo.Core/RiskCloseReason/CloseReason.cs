using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskCloseReason
{
    [Table("CloseReasons")]
    public class CloseReason : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int value { get; set; }

        [Required]
        [StringLength(CloseReasonConsts.MaxnameLength, MinimumLength = CloseReasonConsts.MinnameLength)]
        public virtual string name { get; set; }

    }
}