using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.RiskCategory
{
    [Table("Category")]
    public class Category : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual int Value { get; set; }

        [Required]
        [StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
        public virtual string Name { get; set; }

    }
}