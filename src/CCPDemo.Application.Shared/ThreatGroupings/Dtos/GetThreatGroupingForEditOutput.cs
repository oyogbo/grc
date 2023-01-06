using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace CCPDemo.ThreatGroupings.Dtos
{
    public class GetThreatGroupingForEditOutput
    {
        public CreateOrEditThreatGroupingDto ThreatGrouping { get; set; }

    }
}