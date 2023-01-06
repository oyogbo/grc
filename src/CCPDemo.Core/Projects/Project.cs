using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Projects
{
    [Table("Projects")]
    public class Project : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(ProjectConsts.MaxNameLength, MinimumLength = ProjectConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual DateTime? DueDate { get; set; }

        public virtual int? ConsultantId { get; set; }

        public virtual int? BusinessOwnerId { get; set; }

        public virtual int? DataClassificationId { get; set; }

        public virtual int Order { get; set; }

        [Required]
        public virtual int status { get; set; }

    }
}