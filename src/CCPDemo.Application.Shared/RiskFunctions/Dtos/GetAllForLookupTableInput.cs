using Abp.Application.Services.Dto;

namespace CCPDemo.RiskFunctions.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}