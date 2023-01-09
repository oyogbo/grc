using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace CCPDemo.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var keyRiskIndicators = pages.CreateChildPermission(AppPermissions.Pages_KeyRiskIndicators, L("KeyRiskIndicators"));
            keyRiskIndicators.CreateChildPermission(AppPermissions.Pages_KeyRiskIndicators_Create, L("CreateNewKeyRiskIndicator"));
            keyRiskIndicators.CreateChildPermission(AppPermissions.Pages_KeyRiskIndicators_Edit, L("EditKeyRiskIndicator"));
            keyRiskIndicators.CreateChildPermission(AppPermissions.Pages_KeyRiskIndicators_Delete, L("DeleteKeyRiskIndicator"));

            var risks = pages.CreateChildPermission(AppPermissions.Pages_Risks, L("Risks"));
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Create, L("CreateNewRisk"));
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Edit, L("EditRisk"));
            risks.CreateChildPermission(AppPermissions.Pages_Risks_Delete, L("DeleteRisk"));

            var usersLookups = pages.CreateChildPermission(AppPermissions.Pages_UsersLookups, L("UsersLookups"));
            usersLookups.CreateChildPermission(AppPermissions.Pages_UsersLookups_Create, L("CreateNewUsersLookup"));
            usersLookups.CreateChildPermission(AppPermissions.Pages_UsersLookups_Edit, L("EditUsersLookup"));
            usersLookups.CreateChildPermission(AppPermissions.Pages_UsersLookups_Delete, L("DeleteUsersLookup"));

            var departments = pages.CreateChildPermission(AppPermissions.Pages_Departments, L("Departments"));
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Create, L("CreateNewDepartment"));
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Edit, L("EditDepartment"));
            departments.CreateChildPermission(AppPermissions.Pages_Departments_Delete, L("DeleteDepartment"));

            var assessments = pages.CreateChildPermission(AppPermissions.Pages_Assessments, L("Assessments"));
            assessments.CreateChildPermission(AppPermissions.Pages_Assessments_Create, L("CreateNewAssessment"));
            assessments.CreateChildPermission(AppPermissions.Pages_Assessments_Edit, L("EditAssessment"));
            assessments.CreateChildPermission(AppPermissions.Pages_Assessments_Delete, L("DeleteAssessment"));

            var employees = pages.CreateChildPermission(AppPermissions.Pages_Employees, L("Employees"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Create, L("CreateNewEmployees"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Edit, L("EditEmployees"));
            employees.CreateChildPermission(AppPermissions.Pages_Employees_Delete, L("DeleteEmployees"));

            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            //pages.CreateChildPermission(AppPermissions.Pages_Tenant_PhoneBook, L("PhoneBook"), multiTenancySides: MultiTenancySides.Tenant);

            var phoneBook = pages.CreateChildPermission(AppPermissions.Pages_Tenant_PhoneBook, L("PhoneBook"), multiTenancySides: MultiTenancySides.Tenant);

            phoneBook.CreateChildPermission(AppPermissions.Pages_Tenant_PhoneBook_CreatePerson, L("CreateNewPerson"), multiTenancySides: MultiTenancySides.Tenant);

            phoneBook.CreateChildPermission(AppPermissions.Pages_Tenant_PhoneBook_DeletePerson, L("DeletePerson"), multiTenancySides: MultiTenancySides.Tenant);

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var status = administration.CreateChildPermission(AppPermissions.Pages_Administration_Status, L("Status"));
            status.CreateChildPermission(AppPermissions.Pages_Administration_Status_Create, L("CreateNewStatus"));
            status.CreateChildPermission(AppPermissions.Pages_Administration_Status_Edit, L("EditStatus"));
            status.CreateChildPermission(AppPermissions.Pages_Administration_Status_Delete, L("DeleteStatus"));

            var riskStatus = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskStatus, L("RiskStatus"));
            riskStatus.CreateChildPermission(AppPermissions.Pages_Administration_RiskStatus_Create, L("CreateNewRiskStatus"));
            riskStatus.CreateChildPermission(AppPermissions.Pages_Administration_RiskStatus_Edit, L("EditRiskStatus"));
            riskStatus.CreateChildPermission(AppPermissions.Pages_Administration_RiskStatus_Delete, L("DeleteRiskStatus"));

            var riskRatings = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskRatings, L("RiskRatings"));
            riskRatings.CreateChildPermission(AppPermissions.Pages_Administration_RiskRatings_Create, L("CreateNewRiskRating"));
            riskRatings.CreateChildPermission(AppPermissions.Pages_Administration_RiskRatings_Edit, L("EditRiskRating"));
            riskRatings.CreateChildPermission(AppPermissions.Pages_Administration_RiskRatings_Delete, L("DeleteRiskRating"));

            var riskTypes = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskTypes, L("RiskTypes"));
            riskTypes.CreateChildPermission(AppPermissions.Pages_Administration_RiskTypes_Create, L("CreateNewRiskType"));
            riskTypes.CreateChildPermission(AppPermissions.Pages_Administration_RiskTypes_Edit, L("EditRiskType"));
            riskTypes.CreateChildPermission(AppPermissions.Pages_Administration_RiskTypes_Delete, L("DeleteRiskType"));

            var regulations = administration.CreateChildPermission(AppPermissions.Pages_Administration_Regulations, L("Regulations"));
            regulations.CreateChildPermission(AppPermissions.Pages_Administration_Regulations_Create, L("CreateNewRegulation"));
            regulations.CreateChildPermission(AppPermissions.Pages_Administration_Regulations_Edit, L("EditRegulation"));
            regulations.CreateChildPermission(AppPermissions.Pages_Administration_Regulations_Delete, L("DeleteRegulation"));

            var managementReviews = administration.CreateChildPermission(AppPermissions.Pages_Administration_ManagementReviews, L("ManagementReviews"));
            managementReviews.CreateChildPermission(AppPermissions.Pages_Administration_ManagementReviews_Create, L("CreateNewManagementReview"));
            managementReviews.CreateChildPermission(AppPermissions.Pages_Administration_ManagementReviews_Edit, L("EditManagementReview"));
            managementReviews.CreateChildPermission(AppPermissions.Pages_Administration_ManagementReviews_Delete, L("DeleteManagementReview"));

            var projects = administration.CreateChildPermission(AppPermissions.Pages_Administration_Projects, L("Projects"));
            projects.CreateChildPermission(AppPermissions.Pages_Administration_Projects_Create, L("CreateNewProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Administration_Projects_Edit, L("EditProject"));
            projects.CreateChildPermission(AppPermissions.Pages_Administration_Projects_Delete, L("DeleteProject"));

            var threatGrouping = administration.CreateChildPermission(AppPermissions.Pages_Administration_ThreatGrouping, L("ThreatGrouping"));
            threatGrouping.CreateChildPermission(AppPermissions.Pages_Administration_ThreatGrouping_Create, L("CreateNewThreatGrouping"));
            threatGrouping.CreateChildPermission(AppPermissions.Pages_Administration_ThreatGrouping_Edit, L("EditThreatGrouping"));
            threatGrouping.CreateChildPermission(AppPermissions.Pages_Administration_ThreatGrouping_Delete, L("DeleteThreatGrouping"));

            var threatCatalog = administration.CreateChildPermission(AppPermissions.Pages_Administration_ThreatCatalog, L("ThreatCatalog"));
            threatCatalog.CreateChildPermission(AppPermissions.Pages_Administration_ThreatCatalog_Create, L("CreateNewThreatCatalog"));
            threatCatalog.CreateChildPermission(AppPermissions.Pages_Administration_ThreatCatalog_Edit, L("EditThreatCatalog"));
            threatCatalog.CreateChildPermission(AppPermissions.Pages_Administration_ThreatCatalog_Delete, L("DeleteThreatCatalog"));

            var closures = administration.CreateChildPermission(AppPermissions.Pages_Administration_Closures, L("Closures"));
            closures.CreateChildPermission(AppPermissions.Pages_Administration_Closures_Create, L("CreateNewClosure"));
            closures.CreateChildPermission(AppPermissions.Pages_Administration_Closures_Edit, L("EditClosure"));
            closures.CreateChildPermission(AppPermissions.Pages_Administration_Closures_Delete, L("DeleteClosure"));

            var closeReasons = administration.CreateChildPermission(AppPermissions.Pages_Administration_CloseReasons, L("CloseReasons"));
            closeReasons.CreateChildPermission(AppPermissions.Pages_Administration_CloseReasons_Create, L("CreateNewCloseReason"));
            closeReasons.CreateChildPermission(AppPermissions.Pages_Administration_CloseReasons_Edit, L("EditCloseReason"));
            closeReasons.CreateChildPermission(AppPermissions.Pages_Administration_CloseReasons_Delete, L("DeleteCloseReason"));

            var category = administration.CreateChildPermission(AppPermissions.Pages_Administration_Category, L("Category"));
            category.CreateChildPermission(AppPermissions.Pages_Administration_Category_Create, L("CreateNewCategory"));
            category.CreateChildPermission(AppPermissions.Pages_Administration_Category_Edit, L("EditCategory"));
            category.CreateChildPermission(AppPermissions.Pages_Administration_Category_Delete, L("DeleteCategory"));

            var riskModels = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskModels, L("RiskModels"));
            riskModels.CreateChildPermission(AppPermissions.Pages_Administration_RiskModels_Create, L("CreateNewRiskModel"));
            riskModels.CreateChildPermission(AppPermissions.Pages_Administration_RiskModels_Edit, L("EditRiskModel"));
            riskModels.CreateChildPermission(AppPermissions.Pages_Administration_RiskModels_Delete, L("DeleteRiskModel"));

            var reviewLevels = administration.CreateChildPermission(AppPermissions.Pages_Administration_ReviewLevels, L("ReviewLevels"));
            reviewLevels.CreateChildPermission(AppPermissions.Pages_Administration_ReviewLevels_Create, L("CreateNewReviewLevel"));
            reviewLevels.CreateChildPermission(AppPermissions.Pages_Administration_ReviewLevels_Edit, L("EditReviewLevel"));
            reviewLevels.CreateChildPermission(AppPermissions.Pages_Administration_ReviewLevels_Delete, L("DeleteReviewLevel"));

            var riskLevels = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskLevels, L("RiskLevels"));
            riskLevels.CreateChildPermission(AppPermissions.Pages_Administration_RiskLevels_Create, L("CreateNewRiskLevel"));
            riskLevels.CreateChildPermission(AppPermissions.Pages_Administration_RiskLevels_Edit, L("EditRiskLevel"));
            riskLevels.CreateChildPermission(AppPermissions.Pages_Administration_RiskLevels_Delete, L("DeleteRiskLevel"));

            var riskGroupings = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskGroupings, L("RiskGroupings"));
            riskGroupings.CreateChildPermission(AppPermissions.Pages_Administration_RiskGroupings_Create, L("CreateNewRiskGrouping"));
            riskGroupings.CreateChildPermission(AppPermissions.Pages_Administration_RiskGroupings_Edit, L("EditRiskGrouping"));
            riskGroupings.CreateChildPermission(AppPermissions.Pages_Administration_RiskGroupings_Delete, L("DeleteRiskGrouping"));

            var riskFunctions = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskFunctions, L("RiskFunctions"));
            riskFunctions.CreateChildPermission(AppPermissions.Pages_Administration_RiskFunctions_Create, L("CreateNewRiskFunction"));
            riskFunctions.CreateChildPermission(AppPermissions.Pages_Administration_RiskFunctions_Edit, L("EditRiskFunction"));
            riskFunctions.CreateChildPermission(AppPermissions.Pages_Administration_RiskFunctions_Delete, L("DeleteRiskFunction"));

            var riskCatalogs = administration.CreateChildPermission(AppPermissions.Pages_Administration_RiskCatalogs, L("RiskCatalogs"));
            riskCatalogs.CreateChildPermission(AppPermissions.Pages_Administration_RiskCatalogs_Create, L("CreateNewRiskCatalog"));
            riskCatalogs.CreateChildPermission(AppPermissions.Pages_Administration_RiskCatalogs_Edit, L("EditRiskCatalog"));
            riskCatalogs.CreateChildPermission(AppPermissions.Pages_Administration_RiskCatalogs_Delete, L("DeleteRiskCatalog"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangeProfilePicture, L("UpdateUsersProfilePicture"));

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"));
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeDefaultLanguage, L("ChangeDefaultLanguage"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicProperties = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties, L("DynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Create, L("CreatingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Edit, L("EditingDynamicProperties"));
            dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicProperties_Delete, L("DeletingDynamicProperties"));

            var dynamicPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue, L("DynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Create, L("CreatingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Edit, L("EditingDynamicPropertyValue"));
            dynamicPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicPropertyValue_Delete, L("DeletingDynamicPropertyValue"));

            var dynamicEntityProperties = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties, L("DynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Create, L("CreatingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Edit, L("EditingDynamicEntityProperties"));
            dynamicEntityProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityProperties_Delete, L("DeletingDynamicEntityProperties"));

            var dynamicEntityPropertyValues = dynamicProperties.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue, L("EntityDynamicPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Create, L("CreatingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Edit, L("EditingDynamicEntityPropertyValue"));
            dynamicEntityPropertyValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicEntityPropertyValue_Delete, L("DeletingDynamicEntityPropertyValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CCPDemoConsts.LocalizationSourceName);
        }
    }
}