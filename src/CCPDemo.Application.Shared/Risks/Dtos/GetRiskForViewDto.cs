namespace CCPDemo.Risks.Dtos
{
    public class GetRiskForViewDto
    {
        public RiskDto Risk { get; set; }

        public string RiskTypeName { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string StatusName { get; set; }

        public string RiskRatingName { get; set; }

        public string UserName { get; set; }

    }
}