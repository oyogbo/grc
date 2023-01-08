using CCPDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.UsersLookups.Exporting;
using CCPDemo.UsersLookups.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.UsersLookups
{
    [AbpAuthorize(AppPermissions.Pages_UsersLookups)]
    public class UsersLookupsAppService : CCPDemoAppServiceBase, IUsersLookupsAppService
    {
        private readonly IRepository<UsersLookup> _usersLookupRepository;
        private readonly IUsersLookupsExcelExporter _usersLookupsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;

        public UsersLookupsAppService(IRepository<UsersLookup> usersLookupRepository, IUsersLookupsExcelExporter usersLookupsExcelExporter, IRepository<User, long> lookup_userRepository)
        {
            _usersLookupRepository = usersLookupRepository;
            _usersLookupsExcelExporter = usersLookupsExcelExporter;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetUsersLookupForViewDto>> GetAll(GetAllUsersLookupsInput input)
        {

            var filteredUsersLookups = _usersLookupRepository.GetAll()
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinUserFilter != null, e => e.User >= input.MinUserFilter)
                        .WhereIf(input.MaxUserFilter != null, e => e.User <= input.MaxUserFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredUsersLookups = filteredUsersLookups
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var usersLookups = from o in pagedAndFilteredUsersLookups
                               join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   o.User,
                                   Id = o.Id,
                                   UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredUsersLookups.CountAsync();

            var dbList = await usersLookups.ToListAsync();
            var results = new List<GetUsersLookupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetUsersLookupForViewDto()
                {
                    UsersLookup = new UsersLookupDto
                    {

                        User = o.User,
                        Id = o.Id,
                    },
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetUsersLookupForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetUsersLookupForViewDto> GetUsersLookupForView(int id)
        {
            var usersLookup = await _usersLookupRepository.GetAsync(id);

            var output = new GetUsersLookupForViewDto { UsersLookup = ObjectMapper.Map<UsersLookupDto>(usersLookup) };

            if (output.UsersLookup.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UsersLookup.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UsersLookups_Edit)]
        public async Task<GetUsersLookupForEditOutput> GetUsersLookupForEdit(EntityDto input)
        {
            var usersLookup = await _usersLookupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUsersLookupForEditOutput { UsersLookup = ObjectMapper.Map<CreateOrEditUsersLookupDto>(usersLookup) };

            if (output.UsersLookup.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UsersLookup.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUsersLookupDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_UsersLookups_Create)]
        protected virtual async Task Create(CreateOrEditUsersLookupDto input)
        {
            var usersLookup = ObjectMapper.Map<UsersLookup>(input);

            await _usersLookupRepository.InsertAsync(usersLookup);

        }

        [AbpAuthorize(AppPermissions.Pages_UsersLookups_Edit)]
        protected virtual async Task Update(CreateOrEditUsersLookupDto input)
        {
            var usersLookup = await _usersLookupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, usersLookup);

        }

        [AbpAuthorize(AppPermissions.Pages_UsersLookups_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _usersLookupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUsersLookupsToExcel(GetAllUsersLookupsForExcelInput input)
        {

            var filteredUsersLookups = _usersLookupRepository.GetAll()
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinUserFilter != null, e => e.User >= input.MinUserFilter)
                        .WhereIf(input.MaxUserFilter != null, e => e.User <= input.MaxUserFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredUsersLookups
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetUsersLookupForViewDto()
                         {
                             UsersLookup = new UsersLookupDto
                             {
                                 User = o.User,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var usersLookupListDtos = await query.ToListAsync();

            return _usersLookupsExcelExporter.ExportToFile(usersLookupListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_UsersLookups)]
        public async Task<PagedResultDto<UsersLookupUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UsersLookupUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new UsersLookupUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<UsersLookupUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}