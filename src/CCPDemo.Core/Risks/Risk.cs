using CCPDemo.RiskTypes;
using Abp.Organizations;
using CCPDemo.RiskStatus;
using CCPDemo.RiskRatings;
using CCPDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.Risks
{
    [Table("Risks")]
    public class Risk : FullAuditedEntity
    {

        [Required]
        public virtual string Summary { get; set; }

        public virtual string ExistingControl { get; set; }

        public virtual string ERMRecommendation { get; set; }

        public virtual string ActionPlan { get; set; }

        public virtual string RiskOwnerComment { get; set; }

        public virtual DateTime TargetDate { get; set; }

        public virtual DateTime ActualClosureDate { get; set; }

        public virtual DateTime AcceptanceDate { get; set; }

        public virtual bool RiskAccepted { get; set; }

        public virtual int RiskTypeId { get; set; }

        [ForeignKey("RiskTypeId")]
        public RiskType RiskTypeFk { get; set; }

        public virtual long? OrganizationUnitId { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit OrganizationUnitFk { get; set; }

        public virtual int? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status StatusFk { get; set; }

        public virtual int? RiskRatingId { get; set; }

        [ForeignKey("RiskRatingId")]
        public RiskRating RiskRatingFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}