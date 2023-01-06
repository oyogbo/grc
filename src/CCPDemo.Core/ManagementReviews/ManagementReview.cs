using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.ManagementReviews
{
    [Table("ManagementReviews")]
    public class ManagementReview : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int risk_id { get; set; }

        [Required]
        public virtual DateTime submission_date { get; set; }

        [Required]
        public virtual int review { get; set; }

        [Required]
        public virtual int reviewer { get; set; }

        [Required]
        public virtual int next_step { get; set; }

        [Required]
        public virtual string comments { get; set; }

        [Required]
        public virtual DateTime next_review { get; set; }

    }
}