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
        public virtual string RiskType { get; set; }

        public virtual string RiskSummary { get; set; }

        public virtual string Department { get; set; }

        public virtual long RiskOwnerId { get; set; }
        public virtual string RiskOwner { get; set; }

        public virtual string ExistingControl { get; set; }

        public virtual string ERMComment { get; set; }

        public virtual string ActionPlan { get; set; }

        public virtual string RiskOwnerComment { get; set; }
        public virtual string Status { get; set; }
        public virtual string Rating { get; set; }

        public virtual string TargetDate { get; set; }

        public virtual string ActualClosureDate { get; set; }

        public virtual string AcceptanceDate { get; set; }

        public virtual bool RiskAccepted { get; set; }

    }
}