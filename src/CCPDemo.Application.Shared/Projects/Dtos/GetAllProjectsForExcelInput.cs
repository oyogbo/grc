using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.Projects.Dtos
{
    public class GetAllProjectsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxValueFilter { get; set; }
        public int? MinValueFilter { get; set; }

        public string NameFilter { get; set; }

        public DateTime? MaxDueDateFilter { get; set; }
        public DateTime? MinDueDateFilter { get; set; }

        public int? MaxConsultantIdFilter { get; set; }
        public int? MinConsultantIdFilter { get; set; }

        public int? MaxBusinessOwnerIdFilter { get; set; }
        public int? MinBusinessOwnerIdFilter { get; set; }

        public int? MaxDataClassificationIdFilter { get; set; }
        public int? MinDataClassificationIdFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public int? MaxstatusFilter { get; set; }
        public int? MinstatusFilter { get; set; }

    }
}