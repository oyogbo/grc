using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ManagementReviews.Dtos
{
    public class CreateOrEditManagementReviewDto : EntityDto<int?>
    {

        [Required]
        public int risk_id { get; set; }

        [Required]
        public DateTime submission_date { get; set; }

        [Required]
        public int review { get; set; }

        [Required]
        public int reviewer { get; set; }

        [Required]
        public int next_step { get; set; }

        [Required]
        public string comments { get; set; }

        [Required]
        public DateTime next_review { get; set; }

    }
}