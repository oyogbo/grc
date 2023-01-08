using Abp.Application.Services.Dto;

namespace CCPDemo.RiskRatings.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}