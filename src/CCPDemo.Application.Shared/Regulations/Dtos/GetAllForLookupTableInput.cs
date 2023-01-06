using Abp.Application.Services.Dto;

namespace CCPDemo.Regulations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}