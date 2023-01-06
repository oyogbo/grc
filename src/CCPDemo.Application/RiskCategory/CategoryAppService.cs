using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CCPDemo.RiskCategory.Exporting;
using CCPDemo.RiskCategory.Dtos;
using CCPDemo.Dto;
using Abp.Application.Services.Dto;
using CCPDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using CCPDemo.Storage;

namespace CCPDemo.RiskCategory
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Category)]
    public class CategoryAppService : CCPDemoAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly ICategoryExcelExporter _categoryExcelExporter;

        public CategoryAppService(IRepository<Category> categoryRepository, ICategoryExcelExporter categoryExcelExporter)
        {
            _categoryRepository = categoryRepository;
            _categoryExcelExporter = categoryExcelExporter;

        }

        public async Task<PagedResultDto<GetCategoryForViewDto>> GetAll(GetAllCategoryInput input)
        {

            var filteredCategory = _categoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredCategory = filteredCategory
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var category = from o in pagedAndFilteredCategory
                           select new
                           {

                               o.Value,
                               o.Name,
                               Id = o.Id
                           };

            var totalCount = await filteredCategory.CountAsync();

            var dbList = await category.ToListAsync();
            var results = new List<GetCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCategoryForViewDto()
                {
                    Category = new CategoryDto
                    {

                        Value = o.Value,
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCategoryForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCategoryForViewDto> GetCategoryForView(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            var output = new GetCategoryForViewDto { Category = ObjectMapper.Map<CategoryDto>(category) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Category_Edit)]
        public async Task<GetCategoryForEditOutput> GetCategoryForEdit(EntityDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCategoryForEditOutput { Category = ObjectMapper.Map<CreateOrEditCategoryDto>(category) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Category_Create)]
        protected virtual async Task Create(CreateOrEditCategoryDto input)
        {
            var category = ObjectMapper.Map<Category>(input);

            if (AbpSession.TenantId != null)
            {
                category.TenantId = (int?)AbpSession.TenantId;
            }

            await _categoryRepository.InsertAsync(category);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Category_Edit)]
        protected virtual async Task Update(CreateOrEditCategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, category);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Category_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _categoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCategoryToExcel(GetAllCategoryForExcelInput input)
        {

            var filteredCategory = _categoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(input.MinValueFilter != null, e => e.Value >= input.MinValueFilter)
                        .WhereIf(input.MaxValueFilter != null, e => e.Value <= input.MaxValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredCategory
                         select new GetCategoryForViewDto()
                         {
                             Category = new CategoryDto
                             {
                                 Value = o.Value,
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var categoryListDtos = await query.ToListAsync();

            return _categoryExcelExporter.ExportToFile(categoryListDtos);
        }

    }
}