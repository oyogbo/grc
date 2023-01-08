using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskRatings.Dtos
{
    public class GetAllRiskRatingsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ColorFilter { get; set; }

    }
}