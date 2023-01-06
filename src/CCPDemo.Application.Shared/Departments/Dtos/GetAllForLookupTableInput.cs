using Abp.Application.Services.Dto;

namespace CCPDemo.Departments.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}