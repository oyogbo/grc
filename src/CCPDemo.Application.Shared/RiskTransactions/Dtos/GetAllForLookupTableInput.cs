using Abp.Application.Services.Dto;

namespace CCPDemo.RiskTransactions.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}