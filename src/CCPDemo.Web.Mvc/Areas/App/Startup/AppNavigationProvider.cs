using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using CCPDemo.Authorization;

namespace CCPDemo.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] = new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                    AppPageNames.Host.Tenants,
                    L("Tenants"),
                    url: "App/Tenants",
                    icon: "flaticon-list-3",
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Editions,
                        L("Editions"),
                        url: "App/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.RiskManagement,
                        L("RiskManagement"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Risks,
                        L("Risks"),
                        url: "App/Risks",
                        icon: "flaticon2-indent-dots",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Risks)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Reports,
                        L("Reports"),
                        icon: "flaticon-diagram"
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.OverdueRisks,
                        L("OverdueRisks"),
                        url: "App/Reports/OverdueRisks",
                        icon: "flaticon2-delivery-package",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Risks)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.OnGoingRisks,
                        L("OnGoingRisks"),
                        url: "App/Reports/OnGoingRisks",
                        icon: "flaticon2-layers",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Risks)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.ClosedRisks,
                        L("ClosedRisks"),
                        url: "App/Reports/ClosedRisks",
                        icon: "flaticon2-shrink",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Risks)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.FilteredRisks,
                        L("FilteredRisks"),
                        url: "App/Reports/FilteredRisks",
                        icon: "flaticon2-magnifier-tool",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Risks)
                    )
                )
                )

                 ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.KeyRiskIndicatorManagement,
                        L("KeyRiskIndicatorManagement"),
                        icon: "flaticon-line-graph"
                    ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.UploadKeyRiskIndicator,
                        L("UploadKeyRiskIndicator"),
                        url: "App/UploadKeyRiskIndicator",
                        icon: "flaticon-upload",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_KeyRiskIndicators)
                    )
                    )
                    .AddItem(new MenuItemDefinition(
                       AppPageNames.Common.KeyRiskIndicatorHistory,
                        L("KeyRiskIndicatorHistory"),
                        url: "App/KeyRiskIndicatorHistory",
                        icon: "flaticon-time",
                    //  permissionDependency: new SimplePermissionDependency(AppPermissions.)
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_KeyRiskIndicators)
                    )
                 ))

                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Users)
                        )
                    )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Status,
                        L("Status"),
                        url: "App/Status",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Status)
                    )
                )
                
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.RiskRatings,
                        L("RiskRatings"),
                        url: "App/RiskRatings",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_RiskRatings)
                    )
                )

                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.RiskTypes,
                        L("RiskTypes"),
                        url: "App/RiskTypes",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_RiskTypes)
                    )
                ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_UiCustomization)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Tenant_Settings)
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CCPDemoConsts.LocalizationSourceName);
        }
    }
}