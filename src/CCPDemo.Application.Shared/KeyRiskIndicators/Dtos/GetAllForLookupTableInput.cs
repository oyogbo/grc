using Abp.Application.Services.Dto;

namespace CCPDemo.KeyRiskIndicators.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}