using Abp.Application.Services.Dto;

namespace CCPDemo.Risks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}