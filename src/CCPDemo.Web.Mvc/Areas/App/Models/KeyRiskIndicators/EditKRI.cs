using System.ComponentModel.DataAnnotations;

namespace CCPDemo.Web.Areas.App.Models.KeyRiskIndicators
{
    public class EditKRI
    {
        public virtual int Id { get; set; }
        public virtual string ReferenceId { get; set; }

        public virtual string BusinessLines { get; set; }

        public virtual string Activity { get; set; }

        public virtual string Process { get; set; }

        public virtual string SubProcess { get; set; }

        public virtual string PotentialRisk { get; set; }

        public virtual string LikelihoodOfOccurrence_irr { get; set; }

        public virtual string LikelihoodOfImpact_irr { get; set; }

        public virtual string KeyControl { get; set; }

        public virtual bool IsControlInUse { get; set; }

        public virtual string ControlEffectiveness { get; set; }

        public virtual string LikelihoodOfOccurrence_rrr { get; set; }

        public virtual string LikelihoodOfImpact_rrr { get; set; }

        public virtual string MitigationPlan { get; set; }

        public virtual string Comment { get; set; }

        public virtual string Status { get; set; }

        public virtual string OwnerComment { get; set; }

    }
}
