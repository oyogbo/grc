using Abp.Application.Services.Dto;

namespace CCPDemo.UsersLookups.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}