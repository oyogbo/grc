using Abp.Application.Services.Dto;

namespace CCPDemo.RiskLevels.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}