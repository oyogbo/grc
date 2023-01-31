using CCPDemo.KeyRiskIndicatorHistorys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KeyRiskIndicatorHistories
{
    public interface IKeyRiskIndicatorService
    {
        public Task<KeyRiskIndicatorHistory> AddKeyIndicatorHistory(KRIHistoryAddDTO key);

        public Task<List<KeyRiskIndicatorHistory>> GetAll();
        public Task<bool> UpdateReviewStatus(string referenceId, string status);

        public Task<long> GetUserOrganisationDepartmentId();
        public Task<KeyRiskIndicatorHistory> GetKRIById( int Id);
        public Task<string> GetKRIUploaderEmail(string Id);

        public Task<bool> ApprovedKRIAsync(int Id);
       // Task<bool> DeclineKRIAsync(int Id);

        Task<IList<string>> GetCurrentUserRoles( );


        //Task<bool> DecleteKRIAsync(int Id);

    }
}
