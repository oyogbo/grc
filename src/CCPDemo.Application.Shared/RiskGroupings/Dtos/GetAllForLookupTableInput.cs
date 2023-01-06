using Abp.Application.Services.Dto;

namespace CCPDemo.RiskGroupings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}