using Abp.Application.Services.Dto;

namespace CCPDemo.RiskStatus.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}