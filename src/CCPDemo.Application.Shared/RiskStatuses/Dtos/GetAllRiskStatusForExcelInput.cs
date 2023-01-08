using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskStatuses.Dtos
{
    public class GetAllRiskStatusForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}