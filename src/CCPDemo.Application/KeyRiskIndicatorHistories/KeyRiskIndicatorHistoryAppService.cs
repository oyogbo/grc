using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Organizations;
using CCPDemo.Authorization.Users;
using CCPDemo.EntityFrameworkCore;
using CCPDemo.KeyRiskIndicatorHistorys;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Trunking.V1;

namespace CCPDemo.KeyRiskIndicatorHistories
{
    public class KeyRiskIndicatorHistoryAppService : CCPDemoAppServiceBase, IKeyRiskIndicatorService
    {
        private IKeyRiskIndicatorHistoryRepo _IKeyRiskIndicatorHistoryRepo;
        private readonly IKRIService _iKRIService;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;

        public KeyRiskIndicatorHistoryAppService(IKeyRiskIndicatorHistoryRepo iKeyRiskIndicatorHistoryRepo,
            IKRIService iKRIService,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository

            )
        {
            _IKeyRiskIndicatorHistoryRepo = iKeyRiskIndicatorHistoryRepo;
            _userOrganizationUnitRepository= userOrganizationUnitRepository;
            _iKRIService= iKRIService;
        }

        public async Task<KeyRiskIndicatorHistory> AddKeyIndicatorHistory(KRIHistoryAddDTO model)
        {
            KeyRiskIndicatorHistory keyRiskIndicatorHistory = new KeyRiskIndicatorHistory();
            User user = GetCurrentUser();
            keyRiskIndicatorHistory.UserId = user.Id;
            keyRiskIndicatorHistory.ReferenceId = model.ReferenceId;
            keyRiskIndicatorHistory.BussinessLine = model.BussinessLine;
            keyRiskIndicatorHistory.Department = model.Department;
            keyRiskIndicatorHistory.Status = model.Status;
            keyRiskIndicatorHistory.TotalRecord = model.TotalRecord;
            keyRiskIndicatorHistory.OrganizationUnit = model.OrganizationUnit;
            keyRiskIndicatorHistory.DateCreated= model.DateCreated;
            keyRiskIndicatorHistory.IsReviewed= model.IsReviewed;
            keyRiskIndicatorHistory.ReviewStatus= model.ReviewStatus;

           return   _IKeyRiskIndicatorHistoryRepo.Insert(keyRiskIndicatorHistory);
        }

        
        public async Task<string>GetKRIUploaderEmail(string referenceId)
        {
          var KRIHistory = _IKeyRiskIndicatorHistoryRepo.FirstOrDefault(x => x.ReferenceId == referenceId); 
            if (KRIHistory != null)
            {
                User user = await GetUserByIdAsync(KRIHistory.UserId);
                
                return user.EmailAddress;
            }
            return "";
        }

        public async Task<bool> ApprovedKRIAsync(int Id)
        {
           var KRIFromDB =  _IKeyRiskIndicatorHistoryRepo.Get(Id);
            if (KRIFromDB == null)
                return false;
            KRIFromDB.Status = "Approved";
           var response = _IKeyRiskIndicatorHistoryRepo.Update(KRIFromDB);
            User user = await GetUserByIdAsync(KRIFromDB.UserId);
            List<string>  Emails = new List<string>();
            if (user!= null)
            {
                 Emails = new List<string>();
                Emails.Add(user.EmailAddress);
            }

           bool responseFromEmail = await _iKRIService.ChangeKRIStatusEmailNotificationAsync(Emails, KRIFromDB.ReferenceId, KRIFromDB.Status);
            return responseFromEmail;
                       
        }

        public async Task<bool> DeclineKRIAsync(int Id)
        {
            var KRIFromDB = _IKeyRiskIndicatorHistoryRepo.Get(Id);
            if (KRIFromDB == null)
                return false;
            KRIFromDB.Status = "Declined";
            var response = _IKeyRiskIndicatorHistoryRepo.Update(KRIFromDB);
            User user = await  GetUserByIdAsync(KRIFromDB.UserId);
            List<string> Emails = new List<string>();
            if (user != null)
            {
                Emails = new List<string>();
                Emails.Add(user.EmailAddress);
            }

            bool responseFromEmail = await _iKRIService.ChangeKRIStatusEmailNotificationAsync(Emails, KRIFromDB.ReferenceId, KRIFromDB.Status);
            return responseFromEmail;

        }

        public async Task<List<KeyRiskIndicatorHistory>> GetAll()
        {
            return  _IKeyRiskIndicatorHistoryRepo.GetAllList();
        }

        public async Task<KeyRiskIndicatorHistory> GetKRIById(int Id)
        {
            return _IKeyRiskIndicatorHistoryRepo.Get(Id);
        }

        public Task<IList<string>> GetCurrentUserRoles()
        {
           var roles =  GetCurrentUserRole();
            return roles;
        }

        public async Task<long> GetUserOrganisationDepartmentId()
        {
            User user = GetCurrentUser();

            var ogrunit = _userOrganizationUnitRepository.FirstOrDefault(x => x.UserId == user.Id);
            if (ogrunit != null)
            {
                return ogrunit.OrganizationUnitId;
            }
            return 0;
        }

        public async Task<bool> UpdateReviewStatus(string referenceId, string status)
        {
           var KRIHistory =  await _IKeyRiskIndicatorHistoryRepo.FirstOrDefaultAsync(x => x.ReferenceId== referenceId);
            if (KRIHistory != null)
            {
                KRIHistory.IsReviewed= true;
                KRIHistory.ReviewStatus = status;
                _IKeyRiskIndicatorHistoryRepo.Update(KRIHistory);
                return true;
            }
            return false;
        }
    }
}
