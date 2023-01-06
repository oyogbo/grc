using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Closures.Dtos
{
    public class GetAllClosuresForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxRiskIdFilter { get; set; }
        public int? MinRiskIdFilter { get; set; }

        public int? MaxUserIdFilter { get; set; }
        public int? MinUserIdFilter { get; set; }

        public DateTime? MaxClosureDateFilter { get; set; }
        public DateTime? MinClosureDateFilter { get; set; }

        public int? MaxCloseReasonIdFilter { get; set; }
        public int? MinCloseReasonIdFilter { get; set; }

        public string NoteFilter { get; set; }

    }
}