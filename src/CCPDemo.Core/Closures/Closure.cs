using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Closures
{
    [Table("Closures")]
    public class Closure : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int RiskId { get; set; }

        public virtual int UserId { get; set; }

        public virtual DateTime ClosureDate { get; set; }

        public virtual int CloseReasonId { get; set; }

        public virtual string Note { get; set; }

    }
}