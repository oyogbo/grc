using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskCloseReason.Dtos
{
    public class GetAllCloseReasonsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxvalueFilter { get; set; }
        public int? MinvalueFilter { get; set; }

        public string nameFilter { get; set; }

    }
}