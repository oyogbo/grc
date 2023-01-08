using Abp.Application.Services.Dto;

namespace CCPDemo.RiskStatuses.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}