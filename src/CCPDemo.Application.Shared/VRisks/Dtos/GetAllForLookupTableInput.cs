using Abp.Application.Services.Dto;

namespace CCPDemo.VRisks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}