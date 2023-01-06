using Abp.Application.Services.Dto;

namespace CCPDemo.RiskCatalogs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}