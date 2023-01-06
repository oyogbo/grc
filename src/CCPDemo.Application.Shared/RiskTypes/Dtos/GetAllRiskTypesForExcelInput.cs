using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskTypes.Dtos
{
    public class GetAllRiskTypesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}