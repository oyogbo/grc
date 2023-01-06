using Abp.Application.Services.Dto;

namespace CCPDemo.RiskModels.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}