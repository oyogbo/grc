namespace CCPDemo.KeyRiskIndicators
{
    public class RCSAModel
    {
        public string BussinessLines { get; set; }
        public string Activity { get; set; }
        public string Proccess { get; set; }
        public string SubProcess { get; set; }
        public string PotentailRisk { get; set; }
        public string LikelihoodOfImpact_irr { get; set; }
        public string LikelihoodOfOccurance_irr { get; set; }
        public string KeyControl { get; set; }
        public bool IsControlInUse { get; set; }
        public string ControlOfEffectiveness { get; set; }
        public string LikelihoodOfImpact_rrr { get; set; }
        public string LikelihoodOfOccurance_rrr { get; set; }
        public string MitigationPlan { get; set; }
    }
}