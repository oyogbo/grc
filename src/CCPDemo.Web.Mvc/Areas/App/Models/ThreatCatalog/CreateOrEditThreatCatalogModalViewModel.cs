using CCPDemo.ThreatCatalogs.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.ThreatCatalog
{
    public class CreateOrEditThreatCatalogModalViewModel
    {
        public CreateOrEditThreatCatalogDto ThreatCatalog { get; set; }

        public bool IsEditMode => ThreatCatalog.Id.HasValue;
    }
}