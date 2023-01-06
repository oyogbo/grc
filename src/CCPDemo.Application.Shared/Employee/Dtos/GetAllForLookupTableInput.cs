using Abp.Application.Services.Dto;

namespace CCPDemo.Employee.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}