using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.ManagementReviews.Dtos
{
    public class ManagementReviewDto : EntityDto
    {
        public int risk_id { get; set; }

        public DateTime submission_date { get; set; }

        public int review { get; set; }

        public int reviewer { get; set; }

        public int next_step { get; set; }

        public string comments { get; set; }

        public DateTime next_review { get; set; }

    }
}