using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ManagementReviews.Dtos
{
    public class GetManagementReviewForEditOutput
    {
        public CreateOrEditManagementReviewDto ManagementReview { get; set; }

    }
}