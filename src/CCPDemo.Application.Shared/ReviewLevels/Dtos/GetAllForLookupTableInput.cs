using Abp.Application.Services.Dto;

namespace CCPDemo.ReviewLevels.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}