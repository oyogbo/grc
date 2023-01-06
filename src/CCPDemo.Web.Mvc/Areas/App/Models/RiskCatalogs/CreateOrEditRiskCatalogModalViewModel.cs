using CCPDemo.RiskCatalogs.Dtos;

using Abp.Extensions;

namespace CCPDemo.Web.Areas.App.Models.RiskCatalogs
{
    public class CreateOrEditRiskCatalogModalViewModel
    {
        public CreateOrEditRiskCatalogDto RiskCatalog { get; set; }

        public bool IsEditMode => RiskCatalog.Id.HasValue;
    }
}