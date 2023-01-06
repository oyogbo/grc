using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Regulations.Dtos
{
    public class GetAllRegulationsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxvalueFilter { get; set; }
        public int? MinvalueFilter { get; set; }

        public string nameFilter { get; set; }

    }
}