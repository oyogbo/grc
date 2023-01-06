using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.ThreatGroupings.Dtos
{
    public class GetAllThreatGroupingInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxValueFilter { get; set; }
        public int? MinValueFilter { get; set; }

        public string NameFilter { get; set; }

        public short? MaxDefaultFilter { get; set; }
        public short? MinDefaultFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

    }
}