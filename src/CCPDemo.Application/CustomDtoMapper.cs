using CCPDemo.Risks.Dtos;
using CCPDemo.Risks;
using CCPDemo.RiskTypes.Dtos;
using CCPDemo.RiskTypes;
using CCPDemo.RiskTransactions.Dtos;
using CCPDemo.RiskTransactions;
using CCPDemo.Departments.Dtos;
using CCPDemo.Departments;
using CCPDemo.VRisks.Dtos;
using CCPDemo.VRisks;
using CCPDemo.Regulations.Dtos;
using CCPDemo.Regulations;
using CCPDemo.ManagementReviews.Dtos;
using CCPDemo.ManagementReviews;
using CCPDemo.Projects.Dtos;
using CCPDemo.Projects;
using CCPDemo.Assessments.Dtos;
using CCPDemo.Assessments;
using CCPDemo.ThreatGroupings.Dtos;
using CCPDemo.ThreatGroupings;
using CCPDemo.ThreatCatalogs.Dtos;
using CCPDemo.ThreatCatalogs;
using CCPDemo.Closures.Dtos;
using CCPDemo.Closures;
using CCPDemo.RiskCloseReason.Dtos;
using CCPDemo.RiskCloseReason;
using CCPDemo.RiskCategory.Dtos;
using CCPDemo.RiskCategory;
using CCPDemo.RiskModels.Dtos;
using CCPDemo.RiskModels;
using CCPDemo.ReviewLevels.Dtos;
using CCPDemo.ReviewLevels;
using CCPDemo.RiskLevels.Dtos;
using CCPDemo.RiskLevels;
using CCPDemo.RiskGroupings.Dtos;
using CCPDemo.RiskGroupings;
using CCPDemo.RiskFunctions.Dtos;
using CCPDemo.RiskFunctions;
using CCPDemo.RiskCatalogs.Dtos;
using CCPDemo.RiskCatalogs;
using CCPDemo.Employee.Dtos;
using CCPDemo.Employee;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using CCPDemo.Auditing.Dto;
using CCPDemo.Authorization.Accounts.Dto;
using CCPDemo.Authorization.Delegation;
using CCPDemo.Authorization.Permissions.Dto;
using CCPDemo.Authorization.Roles;
using CCPDemo.Authorization.Roles.Dto;
using CCPDemo.Authorization.Users;
using CCPDemo.Authorization.Users.Delegation.Dto;
using CCPDemo.Authorization.Users.Dto;
using CCPDemo.Authorization.Users.Importing.Dto;
using CCPDemo.Authorization.Users.Profile.Dto;
using CCPDemo.Chat;
using CCPDemo.Chat.Dto;
using CCPDemo.DynamicEntityProperties.Dto;
using CCPDemo.Editions;
using CCPDemo.Editions.Dto;
using CCPDemo.Friendships;
using CCPDemo.Friendships.Cache;
using CCPDemo.Friendships.Dto;
using CCPDemo.Localization.Dto;
using CCPDemo.MultiTenancy;
using CCPDemo.MultiTenancy.Dto;
using CCPDemo.MultiTenancy.HostDashboard.Dto;
using CCPDemo.MultiTenancy.Payments;
using CCPDemo.MultiTenancy.Payments.Dto;
using CCPDemo.Notifications.Dto;
using CCPDemo.Organizations.Dto;
using CCPDemo.Sessions.Dto;
using CCPDemo.WebHooks.Dto;
using CCPDemo.Dto;
using CCPDemo.Persons;
using CCPDemo.Phones;
using CCPDemo.PhoneTypeEntityDir;

namespace CCPDemo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditRiskDto, Risk>().ReverseMap();
            configuration.CreateMap<RiskDto, Risk>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskTypeDto, RiskType>().ReverseMap();
            configuration.CreateMap<RiskTypeDto, RiskType>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskTransactionDto, RiskTransaction>().ReverseMap();
            configuration.CreateMap<RiskTransactionDto, RiskTransaction>().ReverseMap();
            configuration.CreateMap<CreateOrEditDepartmentDto, Department>().ReverseMap();
            configuration.CreateMap<DepartmentDto, Department>().ReverseMap();
            configuration.CreateMap<CreateOrEditVRiskDto, VRisk>().ReverseMap();
            configuration.CreateMap<VRiskDto, VRisk>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegulationDto, Regulation>().ReverseMap();
            configuration.CreateMap<RegulationDto, Regulation>().ReverseMap();
            configuration.CreateMap<CreateOrEditManagementReviewDto, ManagementReview>().ReverseMap();
            configuration.CreateMap<ManagementReviewDto, ManagementReview>().ReverseMap();
            configuration.CreateMap<CreateOrEditProjectDto, Project>().ReverseMap();
            configuration.CreateMap<ProjectDto, Project>().ReverseMap();
            configuration.CreateMap<CreateOrEditAssessmentDto, Assessment>().ReverseMap();
            configuration.CreateMap<AssessmentDto, Assessment>().ReverseMap();
            configuration.CreateMap<CreateOrEditThreatGroupingDto, ThreatGrouping>().ReverseMap();
            configuration.CreateMap<ThreatGroupingDto, ThreatGrouping>().ReverseMap();
            configuration.CreateMap<CreateOrEditThreatCatalogDto, ThreatCatalog>().ReverseMap();
            configuration.CreateMap<ThreatCatalogDto, ThreatCatalog>().ReverseMap();
            configuration.CreateMap<CreateOrEditClosureDto, Closure>().ReverseMap();
            configuration.CreateMap<ClosureDto, Closure>().ReverseMap();
            configuration.CreateMap<CreateOrEditCloseReasonDto, CloseReason>().ReverseMap();
            configuration.CreateMap<CloseReasonDto, CloseReason>().ReverseMap();
            configuration.CreateMap<CreateOrEditCategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CategoryDto, Category>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskModelDto, RiskModel>().ReverseMap();
            configuration.CreateMap<RiskModelDto, RiskModel>().ReverseMap();
            configuration.CreateMap<CreateOrEditReviewLevelDto, ReviewLevel>().ReverseMap();
            configuration.CreateMap<ReviewLevelDto, ReviewLevel>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskLevelDto, RiskLevel>().ReverseMap();
            configuration.CreateMap<RiskLevelDto, RiskLevel>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskGroupingDto, RiskGrouping>().ReverseMap();
            configuration.CreateMap<RiskGroupingDto, RiskGrouping>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskFunctionDto, RiskFunction>().ReverseMap();
            configuration.CreateMap<RiskFunctionDto, RiskFunction>().ReverseMap();
            configuration.CreateMap<CreateOrEditRiskCatalogDto, RiskCatalog>().ReverseMap();
            configuration.CreateMap<RiskCatalogDto, RiskCatalog>().ReverseMap();
            configuration.CreateMap<CreateOrEditEmployeesDto, Employees>().ReverseMap();
            configuration.CreateMap<EmployeesDto, Employees>().ReverseMap();
            //Inputs
            configuration.CreateMap<PhoneType, PhoneTypeListDto>();
            configuration.CreateMap<Person, GetPersonForEditOutput>();
            configuration.CreateMap<AddPhoneInput, PhonePb>();
            configuration.CreateMap<PhonePb, PhoneInPersonListDto>();
            configuration.CreateMap<CreatePersonInput, Person>();
            configuration.CreateMap<Person, PersonListDto>();
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();

            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();

            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}