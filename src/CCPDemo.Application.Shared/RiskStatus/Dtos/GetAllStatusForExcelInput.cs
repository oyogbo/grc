using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskStatus.Dtos
{
    public class GetAllStatusForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}