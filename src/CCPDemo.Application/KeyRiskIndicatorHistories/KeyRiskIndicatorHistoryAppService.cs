using Abp.EntityFrameworkCore;
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
        public KeyRiskIndicatorHistoryAppService(IKeyRiskIndicatorHistoryRepo iKeyRiskIndicatorHistoryRepo, IKRIService iKRIService)
        {
            _IKeyRiskIndicatorHistoryRepo = iKeyRiskIndicatorHistoryRepo;
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
    }
}
