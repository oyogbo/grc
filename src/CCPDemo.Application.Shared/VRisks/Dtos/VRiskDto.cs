using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.VRisks.Dtos
{
    public class VRiskDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Department { get; set; }

        public string RiskOwner { get; set; }

        public string ResolutionTimeLine { get; set; }

        public string ERMComment { get; set; }

        public string RiskOwnerComment { get; set; }

        public string Status { get; set; }
        public string Rating { get; set; }

        public string ActualClosureDate { get; set; }

        public string MitigationDate { get; set; }

        public bool AcceptRisk { get; set; }

        public string AcceptanceDate { get; set; }

    }
}