using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.VRisks.Dtos
{
    public class GetVRiskForEditOutput
    {
        public CreateOrEditVRiskDto VRisk { get; set; }

    }
}