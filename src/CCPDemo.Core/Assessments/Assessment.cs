using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Assessments
{
    [Table("assessments")]
    public class Assessment : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(AssessmentConsts.MaxNameLength, MinimumLength = AssessmentConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}