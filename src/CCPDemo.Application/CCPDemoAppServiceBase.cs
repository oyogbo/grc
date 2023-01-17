using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using Microsoft.AspNetCore.Identity;
using CCPDemo.Authorization.Users;
using CCPDemo.MultiTenancy;
using System.Collections.Generic;
using System.Linq;
using Abp.Organizations;
using System.Linq.Dynamic.Core;

namespace CCPDemo
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class CCPDemoAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected CCPDemoAppServiceBase()
        {
            LocalizationSourceName = CCPDemoConsts.LocalizationSourceName;
        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        public virtual async Task<IList<string>> GetCurrentUserRole()
        {
            User user = GetCurrentUser();
            var roles = await UserManager.GetRolesAsync(user);

            return roles;
        }

        public virtual async Task<IList<string>> GetCurrentUserOrgUnit()
        {
            User user = GetCurrentUser();
            var roles = await UserManager.GetRolesAsync(user);
            return roles;
        }

        protected virtual User GetCurrentUser()
        {
            return AsyncHelper.RunSync(GetCurrentUserAsync);
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }


        protected virtual async Task<User> GetUserByIdAsync(long Id)
        {
            var user = await UserManager.FindByIdAsync(Id.ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual async Task<List<User>> GetAllErm()
        {
            var users = await UserManager.GetUsersInRoleAsync("c593c83d0f6b4463a6997eea9e7f45d7");
            return users.ToList();
        }

        protected virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}