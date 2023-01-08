using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.RiskStatus.Dtos
{
    public class GetAllStatusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}