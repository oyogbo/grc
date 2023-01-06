using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCategory.Dtos
{
    public class GetCategoryForEditOutput
    {
        public CreateOrEditCategoryDto Category { get; set; }

    }
}