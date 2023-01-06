using Abp.Application.Services.Dto;

namespace CCPDemo.ManagementReviews.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}