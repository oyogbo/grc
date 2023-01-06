using Abp.Application.Services.Dto;

namespace CCPDemo.RiskTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}