using Abp.Application.Services.Dto;

namespace CCPDemo.RiskCloseReason.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}