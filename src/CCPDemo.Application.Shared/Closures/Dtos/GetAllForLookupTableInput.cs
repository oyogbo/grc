using Abp.Application.Services.Dto;

namespace CCPDemo.Closures.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}