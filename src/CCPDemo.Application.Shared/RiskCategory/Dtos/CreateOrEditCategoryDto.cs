using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCategory.Dtos
{
    public class CreateOrEditCategoryDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(CategoryConsts.MaxNameLength, MinimumLength = CategoryConsts.MinNameLength)]
        public string Name { get; set; }

    }
}