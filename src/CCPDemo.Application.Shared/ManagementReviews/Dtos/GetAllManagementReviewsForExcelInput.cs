using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.ManagementReviews.Dtos
{
    public class GetAllManagementReviewsForExcelInput
    {
        public string Filter { get; set; }

        public int? Maxrisk_idFilter { get; set; }
        public int? Minrisk_idFilter { get; set; }

        public DateTime? Maxsubmission_dateFilter { get; set; }
        public DateTime? Minsubmission_dateFilter { get; set; }

        public int? MaxreviewFilter { get; set; }
        public int? MinreviewFilter { get; set; }

        public int? MaxreviewerFilter { get; set; }
        public int? MinreviewerFilter { get; set; }

        public int? Maxnext_stepFilter { get; set; }
        public int? Minnext_stepFilter { get; set; }

        public string commentsFilter { get; set; }

        public DateTime? Maxnext_reviewFilter { get; set; }
        public DateTime? Minnext_reviewFilter { get; set; }

    }
}