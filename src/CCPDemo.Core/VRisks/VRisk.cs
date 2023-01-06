using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace CCPDemo.VRisks
{
    [Table("vRisks")]
    public class VRisk : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Department { get; set; }

        public virtual string RiskOwner { get; set; }

        public virtual string ResolutionTimeLine { get; set; }

        public virtual string ERMComment { get; set; }

        public virtual string RiskOwnerComment { get; set; }

        public virtual string Status { get; set; }
        public virtual string Rating { get; set; }

        public virtual string ActualClosureDate { get; set; }

        public virtual string MitigationDate { get; set; }

        public virtual bool AcceptRisk { get; set; }

        public virtual string AcceptanceDate { get; set; }

    }
}