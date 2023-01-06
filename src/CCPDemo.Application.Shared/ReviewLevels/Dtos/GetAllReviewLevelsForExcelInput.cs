using Abp.Application.Services.Dto;
using System;

namespace CCPDemo.ReviewLevels.Dtos
{
    public class GetAllReviewLevelsForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxValueFilter { get; set; }
        public int? MinValueFilter { get; set; }

        public string NameFilter { get; set; }

    }
}