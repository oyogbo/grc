using Abp.Application.Services.Dto;

namespace CCPDemo.ThreatCatalogs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}