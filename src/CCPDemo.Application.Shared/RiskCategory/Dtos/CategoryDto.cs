using System;
using Abp.Application.Services.Dto;

namespace CCPDemo.RiskCategory.Dtos
{
    public class CategoryDto : EntityDto
    {
        public int Value { get; set; }

        public string Name { get; set; }

    }
}