using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.Dto;
using System.Collections.Generic;
using System.IO;


namespace CCPDemo.KeyRiskIndicators
{
    public interface IKeyRiskIndicatorsAppService : IApplicationService
    {
        Task<PagedResultDto<GetKeyRiskIndicatorForViewDto>> GetAll(GetAllKeyRiskIndicatorsInput input);
        Task<GetKeyRiskIndicatorForViewDto> GetAllByRefId(string RefId);
        Task<GetKeyRiskIndicatorForViewDto> GetKeyRiskIndicatorForView(int id);

        Task<GetKeyRiskIndicatorForEditOutput> GetKeyRiskIndicatorForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditKeyRiskIndicatorDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetKeyRiskIndicatorsToExcel(GetAllKeyRiskIndicatorsForExcelInput input);

        IEnumerable<T> ReadCSV<T>(Stream file);
        void WriteCSV<T>(List<T> records);

        Task<bool> DeclineKRI(int Id);
        Task<bool> ApproveclineKRI(List<int> Id);
        Task<List<string>> GetERMEmails();


    }
}