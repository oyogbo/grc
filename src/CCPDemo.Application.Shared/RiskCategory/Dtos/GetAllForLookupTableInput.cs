﻿using Abp.Application.Services.Dto;

namespace CCPDemo.RiskCategory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}