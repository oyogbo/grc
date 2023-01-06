using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.ReviewLevels
{
    [Table("ReviewLevels")]
    public class ReviewLevel : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(ReviewLevelConsts.MaxNameLength, MinimumLength = ReviewLevelConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}