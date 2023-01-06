using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.RiskCloseReason.Dtos
{
    public class CreateOrEditCloseReasonDto : EntityDto<int?>
    {

        [Required]
        public int value { get; set; }

        [Required]
        [StringLength(CloseReasonConsts.MaxnameLength, MinimumLength = CloseReasonConsts.MinnameLength)]
        public string name { get; set; }

    }
}