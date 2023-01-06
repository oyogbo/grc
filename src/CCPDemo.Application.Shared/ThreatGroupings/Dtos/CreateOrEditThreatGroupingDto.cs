using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ThreatGroupings.Dtos
{
    public class CreateOrEditThreatGroupingDto : EntityDto<int?>
    {

        public int Value { get; set; }

        [Required]
        [StringLength(ThreatGroupingConsts.MaxNameLength, MinimumLength = ThreatGroupingConsts.MinNameLength)]
        public string Name { get; set; }

        public short Default { get; set; }

        public int Order { get; set; }

    }
}