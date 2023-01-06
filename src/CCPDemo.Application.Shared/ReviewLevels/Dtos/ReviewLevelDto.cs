using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.ReviewLevels.Dtos
{
    public class ReviewLevelDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

    }
}