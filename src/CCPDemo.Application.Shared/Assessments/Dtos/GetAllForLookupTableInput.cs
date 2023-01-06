using Abp.Application.Services.Dto;

namespace CCPDemo.Assessments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}